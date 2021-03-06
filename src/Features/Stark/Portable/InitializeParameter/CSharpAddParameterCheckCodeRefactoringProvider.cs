﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Composition;
using StarkPlatform.CodeAnalysis.CodeRefactorings;
using StarkPlatform.CodeAnalysis.Stark.Syntax;
using StarkPlatform.CodeAnalysis.Editing;
using StarkPlatform.CodeAnalysis.InitializeParameter;

namespace StarkPlatform.CodeAnalysis.Stark.InitializeParameter
{
    [ExportCodeRefactoringProvider(LanguageNames.Stark, Name = nameof(CSharpAddParameterCheckCodeRefactoringProvider)), Shared]
    [ExtensionOrder(Before = PredefinedCodeRefactoringProviderNames.ChangeSignature)]
    internal class CSharpAddParameterCheckCodeRefactoringProvider :
        AbstractAddParameterCheckCodeRefactoringProvider<
            ParameterSyntax,
            StatementSyntax,
            ExpressionSyntax,
            BinaryExpressionSyntax>
    {
        protected override bool IsFunctionDeclaration(SyntaxNode node)
            => InitializeParameterHelpers.IsFunctionDeclaration(node);

        protected override SyntaxNode GetTypeBlock(SyntaxNode node)
            => node;

        protected override SyntaxNode GetBody(SyntaxNode functionDeclaration)
            => InitializeParameterHelpers.GetBody(functionDeclaration);

        protected override void InsertStatement(SyntaxEditor editor, SyntaxNode functionDeclaration, IMethodSymbol method, SyntaxNode statementToAddAfterOpt, StatementSyntax statement)
            => InitializeParameterHelpers.InsertStatement(editor, functionDeclaration, method, statementToAddAfterOpt, statement);

        protected override bool IsImplicitConversion(Compilation compilation, ITypeSymbol source, ITypeSymbol destination)
            => InitializeParameterHelpers.IsImplicitConversion(compilation, source, destination);

        protected override bool CanOffer(SyntaxNode body)
        {
            if (InitializeParameterHelpers.IsExpressionBody(body))
            {
                return InitializeParameterHelpers.TryConvertExpressionBodyToStatement(body,
                    semicolonToken: SyntaxFactory.Token(SyntaxKind.SemicolonToken),
                    createReturnStatementForExpression: false,
                    statement: out var _);
            }

            return true;
        }
    }
}
