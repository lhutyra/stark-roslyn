// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using StarkPlatform.CodeAnalysis.Stark.Extensions;
using StarkPlatform.CodeAnalysis.Stark.Syntax;
using StarkPlatform.CodeAnalysis.Editor.Implementation.Highlighting;
using StarkPlatform.CodeAnalysis.Text;

namespace StarkPlatform.CodeAnalysis.Editor.CSharp.KeywordHighlighting.KeywordHighlighters
{
    [ExportHighlighter(LanguageNames.Stark)]
    internal class RegionHighlighter : AbstractKeywordHighlighter<DirectiveTriviaSyntax>
    {
        protected override IEnumerable<TextSpan> GetHighlights(
            DirectiveTriviaSyntax directive, CancellationToken cancellationToken)
        {
            var matchingDirective = directive.GetMatchingDirective(cancellationToken);
            if (matchingDirective == null)
            {
                yield break;
            }

            yield return TextSpan.FromBounds(
                directive.HashToken.SpanStart,
                directive.DirectiveNameToken.Span.End);

            yield return TextSpan.FromBounds(
                matchingDirective.HashToken.SpanStart,
                matchingDirective.DirectiveNameToken.Span.End);
        }
    }
}
