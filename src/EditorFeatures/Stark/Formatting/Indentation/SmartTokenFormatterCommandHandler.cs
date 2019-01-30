// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using StarkPlatform.CodeAnalysis.Stark;
using StarkPlatform.CodeAnalysis.Stark.Syntax;
using StarkPlatform.CodeAnalysis.Editor.Implementation.Formatting.Indentation;
using StarkPlatform.CodeAnalysis.Formatting;
using StarkPlatform.CodeAnalysis.Formatting.Rules;
using StarkPlatform.CodeAnalysis.Options;
using StarkPlatform.CodeAnalysis.Text;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;
using Roslyn.Utilities;
using VSCommanding = Microsoft.VisualStudio.Commanding;

namespace StarkPlatform.CodeAnalysis.Editor.CSharp.Formatting.Indentation
{
    [Export(typeof(VSCommanding.ICommandHandler))]
    [ContentType(ContentTypeNames.StarkContentType)]
    [Name(PredefinedCommandHandlerNames.Indent)]
    [Order(After = PredefinedCommandHandlerNames.Rename)]
    [Order(Before = PredefinedCommandHandlerNames.Completion)]
    internal class SmartTokenFormatterCommandHandler :
        AbstractSmartTokenFormatterCommandHandler
    {
        [ImportingConstructor]
        public SmartTokenFormatterCommandHandler(
            ITextUndoHistoryRegistry undoHistoryRegistry,
            IEditorOperationsFactoryService editorOperationsFactoryService) :
            base(undoHistoryRegistry,
                 editorOperationsFactoryService)
        {
        }

        protected override ISmartTokenFormatter CreateSmartTokenFormatter(OptionSet optionSet, IEnumerable<IFormattingRule> formattingRules, SyntaxNode root)
        {
            return new SmartTokenFormatter(optionSet, formattingRules, (CompilationUnitSyntax)root);
        }

        protected override bool UseSmartTokenFormatter(SyntaxNode root, TextLine line, IEnumerable<IFormattingRule> formattingRules, OptionSet options, CancellationToken cancellationToken)
        {
            return CSharpIndentationService.ShouldUseSmartTokenFormatterInsteadOfIndenter(formattingRules, (CompilationUnitSyntax)root, line, options, cancellationToken);
        }

        protected override IEnumerable<IFormattingRule> GetFormattingRules(Document document, int position)
        {
            var workspace = document.Project.Solution.Workspace;
            var formattingRuleFactory = workspace.Services.GetService<IHostDependentFormattingRuleFactoryService>();
            return formattingRuleFactory.CreateRule(document, position).Concat(Formatter.GetDefaultFormattingRules(document));
        }

        protected override bool IsInvalidToken(SyntaxToken token)
        {
            // invalid token to be formatted
            return token.IsKind(SyntaxKind.None) ||
                   token.IsKind(SyntaxKind.EndOfDirectiveToken) ||
                   token.IsKind(SyntaxKind.EndOfFileToken);
        }
    }
}
