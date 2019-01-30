// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using StarkPlatform.CodeAnalysis.Stark.Syntax;
using StarkPlatform.CodeAnalysis.Editor.Implementation.Highlighting;
using StarkPlatform.CodeAnalysis.Text;

namespace StarkPlatform.CodeAnalysis.Editor.CSharp.KeywordHighlighting.KeywordHighlighters
{
    [ExportHighlighter(LanguageNames.Stark)]
    internal class LockStatementHighlighter : AbstractKeywordHighlighter<LockStatementSyntax>
    {
        protected override IEnumerable<TextSpan> GetHighlights(
            LockStatementSyntax lockStatement, CancellationToken cancellationToken)
        {
            yield return lockStatement.LockKeyword.Span;
        }
    }
}
