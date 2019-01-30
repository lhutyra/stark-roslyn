// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Composition;
using StarkPlatform.CodeAnalysis.CodeRefactorings;
using StarkPlatform.CodeAnalysis.Stark.Syntax;
using StarkPlatform.CodeAnalysis.InvertConditional;

namespace StarkPlatform.CodeAnalysis.Stark.InvertConditional
{
    [ExtensionOrder(Before = PredefinedCodeRefactoringProviderNames.IntroduceVariable)]
    [ExportCodeRefactoringProvider(LanguageNames.Stark, Name = PredefinedCodeRefactoringProviderNames.InvertConditional), Shared]
    internal class CSharpInvertConditionalCodeRefactoringProvider
        : AbstractInvertConditionalCodeRefactoringProvider<ConditionalExpressionSyntax>
    {
        // Show the feature in the condition of the conditional up through the ? token.
        // Don't offer if the conditional is missing the colon and the conditional is
        // too incomplete.
        protected override bool ShouldOffer(ConditionalExpressionSyntax conditional, int position)
            => position <= conditional.QuestionToken.Span.End &&
               !conditional.ColonToken.IsMissing;
    }
}
