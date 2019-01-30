// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Stark;
using StarkPlatform.CodeAnalysis.Stark.Extensions;
using StarkPlatform.CodeAnalysis.Stark.Syntax;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.CodeAnalysis.LanguageServices;
using StarkPlatform.CodeAnalysis.Shared.Extensions;
using StarkPlatform.CodeAnalysis.Text;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.F1Help;
using Roslyn.Utilities;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.LanguageService
{
    [ExportLanguageService(typeof(IHelpContextService), LanguageNames.Stark), Shared]
    internal class CSharpHelpContextService : AbstractHelpContextService
    {
        public override string Language
        {
            get
            {
                return "stark";
            }
        }

        public override string Product
        {
            get
            {
                return "stark";
            }
        }

        private static string Keyword(string text)
        {
            return text + "_starkKeyword";
        }

        public override async Task<string> GetHelpTermAsync(Document document, TextSpan span, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            var syntaxFacts = document.GetLanguageService<ISyntaxFactsService>();

            // For now, find the token under the start of the selection.
            var syntaxTree = await document.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
            var token = await syntaxTree.GetTouchingTokenAsync(span.Start, cancellationToken, findInsideTrivia: true).ConfigureAwait(false);

            if (IsValid(token, span))
            {
                var semanticModel = await document.GetSemanticModelForSpanAsync(span, cancellationToken).ConfigureAwait(false);

                var result = TryGetText(token, semanticModel, document, syntaxFacts, cancellationToken);
                if (string.IsNullOrEmpty(result))
                {
                    var previousToken = token.GetPreviousToken();
                    if (IsValid(previousToken, span))
                    {
                        result = TryGetText(previousToken, semanticModel, document, syntaxFacts, cancellationToken);
                    }
                }

                return result;
            }

            var trivia = root.FindTrivia(span.Start, findInsideTrivia: true);
            if (trivia.Span.IntersectsWith(span) && trivia.Kind() == SyntaxKind.PreprocessingMessageTrivia &&
                trivia.Token.GetAncestor<RegionDirectiveTriviaSyntax>() != null)
            {
                return "#region";
            }

            if (trivia.IsRegularOrDocComment())
            {
                // just find the first "word" that intersects with our position
                var text = await syntaxTree.GetTextAsync(cancellationToken).ConfigureAwait(false);
                int start = span.Start;
                int end = span.Start;

                while (start > 0 && syntaxFacts.IsIdentifierPartCharacter(text[start - 1]))
                {
                    start--;
                }

                while (end < text.Length - 1 && syntaxFacts.IsIdentifierPartCharacter(text[end]))
                {
                    end++;
                }

                return text.GetSubText(TextSpan.FromBounds(start, end)).ToString();
            }

            return string.Empty;
        }

        private bool IsValid(SyntaxToken token, TextSpan span)
        {
            // If the token doesn't actually intersect with our position, give up
            return token.Kind() == SyntaxKind.EndIfDirectiveTrivia || token.Span.IntersectsWith(span);
        }

        private string TryGetText(SyntaxToken token, SemanticModel semanticModel, Document document, ISyntaxFactsService syntaxFacts, CancellationToken cancellationToken)
        {
            if (TryGetTextForContextualKeyword(token, document, syntaxFacts, out var text) ||
               TryGetTextForKeyword(token, document, syntaxFacts, out text) ||
               TryGetTextForPreProcessor(token, document, syntaxFacts, out text) ||
               TryGetTextForSymbol(token, semanticModel, document, cancellationToken, out text) ||
               TryGetTextForOperator(token, document, out text))
            {
                return text;
            }

            return string.Empty;
        }

        private bool TryGetTextForSymbol(SyntaxToken token, SemanticModel semanticModel, Document document, CancellationToken cancellationToken, out string text)
        {
            ISymbol symbol;
            if (token.Parent is TypeArgumentListSyntax)
            {
                var genericName = token.GetAncestor<GenericNameSyntax>();
                symbol = semanticModel.GetSymbolInfo(genericName, cancellationToken).Symbol ?? semanticModel.GetTypeInfo(genericName, cancellationToken).Type;
            }
            else if (token.Parent is NullableTypeSyntax && token.IsKind(SyntaxKind.QuestionToken))
            {
                text = "System.Nullable`1";
                return true;
            }
            else
            {
                symbol = semanticModel.GetSemanticInfo(token, document.Project.Solution.Workspace, cancellationToken)
                                      .GetAnySymbol(includeType: true);

                if (symbol == null)
                {
                    var bindableParent = document.GetLanguageService<ISyntaxFactsService>().GetBindableParent(token);
                    var overloads = semanticModel.GetMemberGroup(bindableParent);
                    symbol = overloads.FirstOrDefault();
                }
            }

            // Local: return the name if it's the declaration, otherwise the type
            if (symbol is ILocalSymbol && !symbol.DeclaringSyntaxReferences.Any(d => d.GetSyntax().DescendantTokens().Contains(token)))
            {
                symbol = ((ILocalSymbol)symbol).Type;
            }

            // Range variable: use the type
            if (symbol is IRangeVariableSymbol)
            {
                var info = semanticModel.GetTypeInfo(token.Parent, cancellationToken);
                symbol = info.Type;
            }

            // Just use syntaxfacts for operators
            if (symbol is IMethodSymbol && ((IMethodSymbol)symbol).MethodKind == MethodKind.BuiltinOperator)
            {
                text = null;
                return false;
            }

            text = symbol != null ? FormatSymbol(symbol) : null;
            return symbol != null;
        }

        private bool TryGetTextForOperator(SyntaxToken token, Document document, out string text)
        {
            var syntaxFacts = document.GetLanguageService<ISyntaxFactsService>();
            if (syntaxFacts.IsOperator(token) || syntaxFacts.IsPredefinedOperator(token) || SyntaxFacts.IsAssignmentExpressionOperatorToken(token.Kind()))
            {
                text = Keyword(syntaxFacts.GetText(token.RawKind));
                return true;
            }

            if (token.IsKind(SyntaxKind.ColonColonToken))
            {
                text = "::_starkKeyword";
                return true;
            }

            if (token.Kind() == SyntaxKind.ColonToken && token.Parent is NameColonSyntax)
            {
                text = "cs_namedParameter";
                return true;
            }

            if (token.IsKind(SyntaxKind.QuestionToken) && token.Parent is ConditionalExpressionSyntax)
            {
                text = "?_starkKeyword";
                return true;
            }

            if (token.IsKind(SyntaxKind.EqualsGreaterThanToken))
            {
                text = "=>_starkKeyword";
                return true;
            }

            if (token.IsKind(SyntaxKind.PlusEqualsToken))
            {
                text = "+=_starkKeyword";
                return true;
            }

            if (token.IsKind(SyntaxKind.MinusEqualsToken))
            {
                text = "-=_starkKeyword";
                return true;
            }

            text = null;
            return false;
        }

        private bool TryGetTextForPreProcessor(SyntaxToken token, Document document, ISyntaxFactsService syntaxFacts, out string text)
        {
            if (syntaxFacts.IsPreprocessorKeyword(token))
            {
                text = "#" + token.Text;
                return true;
            }

            if (token.IsKind(SyntaxKind.EndOfDirectiveToken) && token.GetAncestor<RegionDirectiveTriviaSyntax>() != null)
            {
                text = "#region";
                return true;
            }

            text = null;
            return false;
        }

        private bool TryGetTextForContextualKeyword(SyntaxToken token, Document document, ISyntaxFactsService syntaxFacts, out string text)
        {
            if (token.IsContextualKeyword())
            {
                switch (token.Kind())
                {
                    case SyntaxKind.PartialKeyword:
                        if (token.Parent.GetAncestorOrThis<MethodDeclarationSyntax>() != null)
                        {
                            text = "partialmethod_starkKeyword";
                            return true;
                        }
                        else if (token.Parent.GetAncestorOrThis<ClassDeclarationSyntax>() != null)
                        {
                            text = "partialtype_starkKeyword";
                            return true;
                        }

                        break;

                    case SyntaxKind.WhereKeyword:
                        if (token.Parent.GetAncestorOrThis<TypeParameterConstraintClauseSyntax>() != null)
                        {
                            text = "whereconstraint_starkKeyword";
                        }
                        else
                        {
                            text = "whereclause_starkKeyword";
                        }

                        return true;
                }
            }

            text = null;
            return false;
        }

        private bool TryGetTextForKeyword(SyntaxToken token, Document document, ISyntaxFactsService syntaxFacts, out string text)
        {
            if (token.Kind() == SyntaxKind.InKeyword)
            {
                if (token.GetAncestor<FromClauseSyntax>() != null)
                {
                    text = "from_starkKeyword";
                    return true;
                }

                if (token.GetAncestor<JoinClauseSyntax>() != null)
                {
                    text = "join_starkKeyword";
                    return true;
                }
            }

            if (token.IsKeyword())
            {
                text = Keyword(token.Text);
                return true;
            }

            if (token.ValueText == "var" && token.IsKind(SyntaxKind.IdentifierToken) &&
                token.Parent.Parent is VariableDeclarationSyntax && token.Parent == ((VariableDeclarationSyntax)token.Parent.Parent).Type)
            {
                text = "var_starkKeyword";
                return true;
            }

            if (syntaxFacts.IsTypeNamedDynamic(token, token.Parent))
            {
                text = "dynamic_starkKeyword";
                return true;
            }

            text = null;
            return false;
        }

        private static string FormatNamespaceOrTypeSymbol(INamespaceOrTypeSymbol symbol)
        {
            var displayString = symbol.ToDisplayString(TypeFormat);

            if (symbol is ITypeSymbol type && type.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
            {
                return "System.Nullable`1";
            }

            if (symbol.GetTypeArguments().Any())
            {
                return $"{displayString}`{symbol.GetTypeArguments().Length}";
            }

            return displayString;
        }

        public override string FormatSymbol(ISymbol symbol)
        {
            if (symbol is ITypeSymbol || symbol is INamespaceSymbol)
            {
                return FormatNamespaceOrTypeSymbol((INamespaceOrTypeSymbol)symbol);
            }

            if (symbol.MatchesKind(SymbolKind.Alias, SymbolKind.Local, SymbolKind.Parameter))
            {
                return FormatSymbol(symbol.GetSymbolType());
            }

            var containingType = FormatNamespaceOrTypeSymbol(symbol.ContainingType);
            var name = symbol.ToDisplayString(NameFormat);

            if (symbol.IsConstructor())
            {
                return $"{containingType}.#ctor";
            }

            if (symbol.GetTypeArguments().Any())
            {
                return $"{containingType}.{name}``{symbol.GetTypeArguments().Length}";
            }

            return $"{containingType}.{name}";
        }
    }
}
