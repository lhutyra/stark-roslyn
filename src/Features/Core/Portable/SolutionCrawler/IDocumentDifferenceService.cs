﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using StarkPlatform.CodeAnalysis.Host;

namespace StarkPlatform.CodeAnalysis.SolutionCrawler
{
    internal class DocumentDifferenceResult
    {
        public InvocationReasons ChangeType { get; }
        public SyntaxNode ChangedMember { get; }

        public DocumentDifferenceResult(InvocationReasons changeType, SyntaxNode changedMember = null)
        {
            this.ChangeType = changeType;
            this.ChangedMember = changedMember;
        }
    }

    internal interface IDocumentDifferenceService : ILanguageService
    {
        Task<DocumentDifferenceResult> GetDifferenceAsync(Document oldDocument, Document newDocument, CancellationToken cancellationToken);
    }
}
