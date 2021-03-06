﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StarkPlatform.CodeAnalysis.CodeActions;
using StarkPlatform.CodeAnalysis.CodeRefactorings;
using StarkPlatform.CodeAnalysis.LanguageServices;
using StarkPlatform.CodeAnalysis.Shared.Extensions;
using StarkPlatform.CodeAnalysis.PooledObjects;
using StarkPlatform.CodeAnalysis.Shared.Utilities;
using Roslyn.Utilities;

namespace StarkPlatform.CodeAnalysis.AddFileBanner
{
    internal abstract class AbstractAddFileBannerCodeRefactoringProvider : CodeRefactoringProvider
    {
        protected abstract bool IsCommentStartCharacter(char ch);

        protected abstract SyntaxTrivia CreateTrivia(SyntaxTrivia trivia, string text);

        public override async Task ComputeRefactoringsAsync(CodeRefactoringContext context)
        {
            var cancellationToken = context.CancellationToken;
            var document = context.Document;

            if (!context.Span.IsEmpty)
            {
                return;
            }

            var position = context.Span.Start;
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            var firstToken = root.GetFirstToken();
            if (!firstToken.FullSpan.IntersectsWith(position))
            {
                return;
            }

            var syntaxFacts = document.GetLanguageService<ISyntaxFactsService>();
            var banner = syntaxFacts.GetFileBanner(root);

            if (banner.Length > 0)
            {
                // Already has a banner.
                return;
            }

            // Process the other documents in this document's project.  Look at the
            // ones that we can get a root from (without having to parse).  Then
            // look at the ones we'd need to parse.
            var siblingDocumentsAndRoots =
                document.Project.Documents
                        .Where(d => d != document)
                        .Select(d =>
                        {
                            d.TryGetSyntaxRoot(out var siblingRoot);
                            return (document: d, root: siblingRoot);
                        })
                        .OrderBy((t1, t2) => (t1.root != null) == (t2.root != null) ? 0 : t1.root != null ? -1 : 1);

            foreach (var (siblingDocument, siblingRoot) in siblingDocumentsAndRoots)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var siblingBanner = await TryGetBannerAsync(siblingDocument, siblingRoot, cancellationToken).ConfigureAwait(false);
                if (siblingBanner.Length > 0 && !siblingDocument.IsGeneratedCode(cancellationToken))
                {
                    context.RegisterRefactoring(
                        new MyCodeAction(c => AddBannerAsync(document, root, siblingDocument, siblingBanner, c)));
                    return;
                }
            }
        }

        private Task<Document> AddBannerAsync(
            Document document, SyntaxNode root,
            Document siblingDocument, ImmutableArray<SyntaxTrivia> banner,
            CancellationToken cancellationToken)
        {
            banner = UpdateEmbeddedFileNames(siblingDocument, document, banner);

            var newRoot = root.WithPrependedLeadingTrivia(new SyntaxTriviaList(banner));
            return Task.FromResult(document.WithSyntaxRoot(newRoot));
        }

        /// <summary>
        /// Looks at <paramref name="banner"/> to see if it contains the name of <paramref name="sourceDocument"/>
        /// in it.  If so, those names will be replaced with <paramref name="destinationDocument"/>'s name.
        /// </summary>
        private ImmutableArray<SyntaxTrivia> UpdateEmbeddedFileNames(
            Document sourceDocument, Document destinationDocument, ImmutableArray<SyntaxTrivia> banner)
        {
            var sourceName = IOUtilities.PerformIO(() => Path.GetFileName(sourceDocument.FilePath));
            var destinationName = IOUtilities.PerformIO(() => Path.GetFileName(destinationDocument.FilePath));
            if (string.IsNullOrEmpty(sourceName) || string.IsNullOrEmpty(destinationName))
            {
                return banner;
            }

            var result = ArrayBuilder<SyntaxTrivia>.GetInstance();
            foreach (var trivia in banner)
            {
                var updated = CreateTrivia(trivia, trivia.ToFullString().Replace(sourceName, destinationName));
                result.Add(updated);
            }

            return result.ToImmutableAndFree();
        }

        private async Task<ImmutableArray<SyntaxTrivia>> TryGetBannerAsync(
            Document document, SyntaxNode root, CancellationToken cancellationToken)
        {
            var syntaxFacts = document.GetLanguageService<ISyntaxFactsService>();

            // If we have a tree already for this document, then just check to see
            // if it has a banner.
            if (root != null)
            {
                return syntaxFacts.GetFileBanner(root);
            }

            // Didn't have a tree.  Don't want to parse the file if we can avoid it.
            var text = await document.GetTextAsync(cancellationToken).ConfigureAwait(false);
            if (text.Length == 0 || !IsCommentStartCharacter(text[0]))
            {
                // Didn't start with a comment character, don't bother looking at 
                // this file.
                return ImmutableArray<SyntaxTrivia>.Empty;
            }

            var token = syntaxFacts.ParseToken(text.ToString());
            return syntaxFacts.GetFileBanner(token);
        }

        private class MyCodeAction : CodeAction.DocumentChangeAction
        {
            public MyCodeAction(Func<CancellationToken, Task<Document>> createChangedDocument)
                : base(FeaturesResources.Add_file_banner, createChangedDocument)
            {
            }
        }
    }
}
