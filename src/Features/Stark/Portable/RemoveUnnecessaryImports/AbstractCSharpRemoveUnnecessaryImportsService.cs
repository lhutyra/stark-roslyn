﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StarkPlatform.CodeAnalysis.Stark.Syntax;
using StarkPlatform.CodeAnalysis.Formatting;
using StarkPlatform.CodeAnalysis.Internal.Log;
using StarkPlatform.CodeAnalysis.RemoveUnnecessaryImports;
using StarkPlatform.CodeAnalysis.Shared.Extensions;
using StarkPlatform.CodeAnalysis.Text;
using Roslyn.Utilities;

namespace StarkPlatform.CodeAnalysis.Stark.RemoveUnnecessaryImports
{
    internal partial class AbstractCSharpRemoveUnnecessaryImportsService :
        AbstractRemoveUnnecessaryImportsService<ImportDirectiveSyntax>
    {
        public override async Task<Document> RemoveUnnecessaryImportsAsync(
            Document document,
            Func<SyntaxNode, bool> predicate,
            CancellationToken cancellationToken)
        {
            predicate = predicate ?? Functions<SyntaxNode>.True;
            using (Logger.LogBlock(FunctionId.Refactoring_RemoveUnnecessaryImports_CSharp, cancellationToken))
            {
                var unnecessaryImports = await GetCommonUnnecessaryImportsOfAllContextAsync(
                    document, predicate, cancellationToken).ConfigureAwait(false);
                if (unnecessaryImports == null || unnecessaryImports.Any(import => import.OverlapsHiddenPosition(cancellationToken)))
                {
                    return document;
                }

                var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

                var oldRoot = (CompilationUnitSyntax)root;
                var newRoot = (CompilationUnitSyntax)new Rewriter(this, document, unnecessaryImports, cancellationToken).Visit(oldRoot);

                cancellationToken.ThrowIfCancellationRequested();
                return document.WithSyntaxRoot(await FormatResultAsync(document, newRoot, cancellationToken).ConfigureAwait(false));
            }
        }

        protected override ImmutableArray<ImportDirectiveSyntax> GetUnnecessaryImports(
            SemanticModel model, SyntaxNode root,
            Func<SyntaxNode, bool> predicate, CancellationToken cancellationToken)
        {
            predicate = predicate ?? Functions<SyntaxNode>.True;
            var diagnostics = model.GetDiagnostics(cancellationToken: cancellationToken);
            if (!diagnostics.Any())
            {
                return ImmutableArray<ImportDirectiveSyntax>.Empty;
            }

            var unnecessaryImports = new HashSet<ImportDirectiveSyntax>();

            foreach (var diagnostic in diagnostics)
            {
                if (diagnostic.Id == "CS8019")
                {

                    if (root.FindNode(diagnostic.Location.SourceSpan) is ImportDirectiveSyntax node && predicate(node))
                    {
                        unnecessaryImports.Add(node);
                    }
                }
            }

            return unnecessaryImports.ToImmutableArray();
        }

        private async Task<SyntaxNode> FormatResultAsync(Document document, CompilationUnitSyntax newRoot, CancellationToken cancellationToken)
        {
            var spans = new List<TextSpan>();
            AddFormattingSpans(newRoot, spans, cancellationToken);
            var options = await document.GetOptionsAsync(cancellationToken).ConfigureAwait(false);
            return Formatter.Format(newRoot, spans, document.Project.Solution.Workspace, options, cancellationToken: cancellationToken);
        }

        private void AddFormattingSpans(
            CompilationUnitSyntax compilationUnit,
            List<TextSpan> spans,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            spans.Add(TextSpan.FromBounds(0, GetEndPosition(compilationUnit, compilationUnit.Members)));

            foreach (var @namespace in compilationUnit.Members.OfType<NamespaceDeclarationSyntax>())
            {
                AddFormattingSpans(@namespace, spans, cancellationToken);
            }
        }

        private void AddFormattingSpans(
            NamespaceDeclarationSyntax namespaceMember,
            List<TextSpan> spans,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            spans.Add(TextSpan.FromBounds(namespaceMember.SpanStart, GetEndPosition(namespaceMember, namespaceMember.Members)));

            foreach (var @namespace in namespaceMember.Members.OfType<NamespaceDeclarationSyntax>())
            {
                AddFormattingSpans(@namespace, spans, cancellationToken);
            }
        }

        private int GetEndPosition(SyntaxNode container, SyntaxList<MemberDeclarationSyntax> list)
        {
            return list.Count > 0 ? list[0].SpanStart : container.Span.End;
        }
    }
}
