﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

#if CODE_STYLE
extern alias CodeStyle;
#endif

using System.Threading;
using System.Threading.Tasks;
using StarkPlatform.CodeAnalysis.Text;

#if CODE_STYLE
using Formatter = CodeStyle::StarkPlatform.CodeAnalysis.Formatting.Formatter;
using FormatterState = StarkPlatform.CodeAnalysis.Formatting.ISyntaxFormattingService;
#else
using StarkPlatform.CodeAnalysis.Formatting;
using StarkPlatform.CodeAnalysis.Options;
using FormatterState = StarkPlatform.CodeAnalysis.Workspace;
#endif

namespace StarkPlatform.CodeAnalysis
{
    internal static class FormattingCodeFixHelper
    {
        internal static async Task<SyntaxTree> FixOneAsync(SyntaxTree syntaxTree, FormatterState formatterState, OptionSet options, Diagnostic diagnostic, CancellationToken cancellationToken)
        {
            // The span to format is the full line(s) containing the diagnostic
            var text = await syntaxTree.GetTextAsync(cancellationToken).ConfigureAwait(false);
            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var diagnosticLinePositionSpan = text.Lines.GetLinePositionSpan(diagnosticSpan);
            var spanToFormat = TextSpan.FromBounds(
                text.Lines[diagnosticLinePositionSpan.Start.Line].Start,
                text.Lines[diagnosticLinePositionSpan.End.Line].End);

            var root = await syntaxTree.GetRootAsync(cancellationToken).ConfigureAwait(false);
#if CODE_STYLE
            var formattedRoot = Formatter.Format(root, formatterState, new[] { spanToFormat }, options, Formatter.GetDefaultFormattingRules(formatterState), cancellationToken);
#else
            var formattedRoot = Formatter.Format(root, spanToFormat, formatterState, options, cancellationToken);
#endif

            return syntaxTree.WithRootAndOptions(formattedRoot, syntaxTree.Options);
        }
    }
}
