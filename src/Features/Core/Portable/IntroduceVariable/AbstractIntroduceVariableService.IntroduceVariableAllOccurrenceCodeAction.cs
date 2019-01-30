﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using StarkPlatform.CodeAnalysis.CaseCorrection;
using StarkPlatform.CodeAnalysis.Formatting;
using StarkPlatform.CodeAnalysis.Simplification;

namespace StarkPlatform.CodeAnalysis.IntroduceVariable
{
    internal partial class AbstractIntroduceVariableService<TService, TExpressionSyntax, TTypeSyntax, TTypeDeclarationSyntax, TQueryExpressionSyntax, TNameSyntax>
    {
        private class IntroduceVariableAllOccurrenceCodeAction : AbstractIntroduceVariableCodeAction
        {
            internal IntroduceVariableAllOccurrenceCodeAction(
                TService service,
                SemanticDocument document,
                TExpressionSyntax expression,
                bool allOccurrences,
                bool isConstant,
                bool isLocal,
                bool isQueryLocal)
                : base(service, document, expression, allOccurrences, isConstant, isLocal, isQueryLocal)
            {
            }

            protected override async Task<Document> PostProcessChangesAsync(Document document, CancellationToken cancellationToken)
            {
                document = await Simplifier.ReduceAsync(document, Simplifier.Annotation, cancellationToken: cancellationToken).ConfigureAwait(false);
                document = await Formatter.FormatAsync(document, Formatter.Annotation, cancellationToken: cancellationToken).ConfigureAwait(false);
                document = await CaseCorrector.CaseCorrectAsync(document, CaseCorrector.Annotation, cancellationToken).ConfigureAwait(false);
                return document;
            }
        }
    }
}
