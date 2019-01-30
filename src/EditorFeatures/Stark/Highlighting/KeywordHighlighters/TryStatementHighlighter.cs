// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using StarkPlatform.CodeAnalysis.Stark.Syntax;
using StarkPlatform.CodeAnalysis.Editor.Implementation.Highlighting;
using StarkPlatform.CodeAnalysis.Text;

namespace StarkPlatform.CodeAnalysis.Editor.CSharp.KeywordHighlighting.KeywordHighlighters
{
    [ExportHighlighter(LanguageNames.Stark)]
    internal class TryStatementHighlighter : AbstractKeywordHighlighter<TryStatementSyntax>
    {
        protected override IEnumerable<TextSpan> GetHighlights(
            TryStatementSyntax tryStatement, CancellationToken cancellationToken)
        {
            yield return tryStatement.TryKeyword.Span;

            foreach (var catchDeclaration in tryStatement.Catches)
            {
                yield return catchDeclaration.CatchKeyword.Span;

                if (catchDeclaration.Filter != null)
                {
                    yield return catchDeclaration.Filter.WhenKeyword.Span;
                }
            }

            if (tryStatement.Finally != null)
            {
                yield return tryStatement.Finally.FinallyKeyword.Span;
            }
        }
    }
}
