﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using StarkPlatform.CodeAnalysis.Stark;
using StarkPlatform.CodeAnalysis.Stark.Extensions;
using StarkPlatform.CodeAnalysis.Stark.Syntax;
using StarkPlatform.CodeAnalysis.Text;

namespace StarkPlatform.CodeAnalysis.Editor.CSharp.BraceMatching
{
    [ExportBraceMatcher(LanguageNames.Stark)]
    internal class StringLiteralBraceMatcher : IBraceMatcher
    {
        public async Task<BraceMatchingResult?> FindBracesAsync(Document document, int position, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            var token = root.FindToken(position);

            if (!token.ContainsDiagnostics)
            {
                if (token.IsKind(SyntaxKind.StringLiteralToken))
                {
                    if (token.IsVerbatimStringLiteral())
                    {
                        return new BraceMatchingResult(
                            new TextSpan(token.SpanStart, 2),
                            new TextSpan(token.Span.End - 1, 1));
                    }
                    else
                    {
                        return new BraceMatchingResult(
                            new TextSpan(token.SpanStart, 1),
                            new TextSpan(token.Span.End - 1, 1));
                    }
                }
                else if (token.IsKind(SyntaxKind.InterpolatedStringStartToken, SyntaxKind.InterpolatedVerbatimStringStartToken))
                {
                    if (token.Parent is InterpolatedStringExpressionSyntax interpolatedString)
                    {
                        return new BraceMatchingResult(token.Span, interpolatedString.StringEndToken.Span);
                    }
                }
                else if (token.IsKind(SyntaxKind.InterpolatedStringEndToken))
                {
                    if (token.Parent is InterpolatedStringExpressionSyntax interpolatedString)
                    {
                        return new BraceMatchingResult(interpolatedString.StringStartToken.Span, token.Span);
                    }
                }
            }

            return null;
        }
    }
}
