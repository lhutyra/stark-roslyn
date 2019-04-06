// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using StarkPlatform.CodeAnalysis.Stark.Extensions;
using StarkPlatform.CodeAnalysis.Stark.Syntax;
using StarkPlatform.CodeAnalysis.Editor.Implementation.Highlighting;
using StarkPlatform.CodeAnalysis.Text;

namespace StarkPlatform.CodeAnalysis.Editor.CSharp.KeywordHighlighting.KeywordHighlighters
{
    [ExportHighlighter(LanguageNames.Stark)]
    internal class LoopHighlighter : AbstractKeywordHighlighter
    {
        protected override bool IsHighlightableNode(SyntaxNode node)
            => node.IsContinuableConstruct();

        protected override IEnumerable<TextSpan> GetHighlightsForNode(
            SyntaxNode node, CancellationToken cancellationToken)
        {
            var spans = new List<TextSpan>();

            switch (node)
            {
                case DoStatementSyntax doStatement:
                    HighlightDoStatement(doStatement, spans);
                    break;
                case ForStatementSyntax2 forStatement:
                    HighlightForStatement(forStatement, spans);
                    break;
                case ForStatementSyntax forEachStatement:
                    HighlightForEachStatement(forEachStatement, spans);
                    break;
                case WhileStatementSyntax whileStatement:
                    HighlightWhileStatement(whileStatement, spans);
                    break;
            }

            HighlightRelatedKeywords(node, spans, highlightBreaks: true, highlightContinues: true);

            return spans;
        }

        private void HighlightDoStatement(DoStatementSyntax statement, List<TextSpan> spans)
        {
            spans.Add(statement.DoKeyword.Span);
            spans.Add(statement.WhileKeyword.Span);
            spans.Add(EmptySpan(statement.EosToken.Span.End));
        }

        private void HighlightForStatement(ForStatementSyntax2 statement, List<TextSpan> spans)
        {
            spans.Add(statement.ForKeyword.Span);
        }

        private void HighlightForEachStatement(ForStatementSyntax statement, List<TextSpan> spans)
        {
            spans.Add(statement.ForEachKeyword.Span);
        }

        private void HighlightWhileStatement(WhileStatementSyntax statement, List<TextSpan> spans)
        {
            spans.Add(statement.WhileKeyword.Span);
        }

        /// <summary>
        /// Finds all breaks and continues that are a child of this node, and adds the appropriate spans to the spans list.
        /// </summary>
        private void HighlightRelatedKeywords(SyntaxNode node, List<TextSpan> spans,
            bool highlightBreaks, bool highlightContinues)
        {
            Debug.Assert(highlightBreaks || highlightContinues);

            if (highlightBreaks && node is BreakStatementSyntax breakStatement)
            {
                spans.Add(breakStatement.BreakKeyword.Span);
                spans.Add(EmptySpan(breakStatement.EosToken.Span.End));
            }
            else if (highlightContinues && node is ContinueStatementSyntax continueStatement)
            {
                spans.Add(continueStatement.ContinueKeyword.Span);
                spans.Add(EmptySpan(continueStatement.EosToken.Span.End));
            }
            else
            {
                foreach (var child in node.ChildNodes())
                {
                    var highlightBreaksForChild = highlightBreaks && !child.IsBreakableConstruct();
                    var highlightContinuesForChild = highlightContinues && !child.IsContinuableConstruct();

                    // Only recurse if we have anything to do
                    if (highlightBreaksForChild || highlightContinuesForChild)
                    {
                        HighlightRelatedKeywords(child, spans, highlightBreaksForChild, highlightContinuesForChild);
                    }
                }
            }
        }
    }
}
