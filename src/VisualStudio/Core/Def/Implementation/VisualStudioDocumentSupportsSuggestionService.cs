﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Composition;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Editor.Shared;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.ProjectSystem;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.Venus;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.SuggestionService
{
    [ExportWorkspaceService(typeof(IDocumentSupportsFeatureService), ServiceLayer.Host), Shared]
    internal sealed class VisualStudioDocumentSupportsFeatureService : IDocumentSupportsFeatureService
    {
        public bool SupportsCodeFixes(Document document)
        {
            return GetContainedDocument(document) == null;
        }

        public bool SupportsRefactorings(Document document)
        {
            return GetContainedDocument(document) == null;
        }

        public bool SupportsRename(Document document)
        {
            var containedDocument = GetContainedDocument(document);
            return containedDocument == null || containedDocument.SupportsRename;
        }

        public bool SupportsNavigationToAnyPosition(Document document)
        {
            return GetContainedDocument(document) == null;
        }

        private static ContainedDocument GetContainedDocument(Document document)
        {
            var visualStudioWorkspace = document.Project.Solution.Workspace as VisualStudioWorkspaceImpl;
            return visualStudioWorkspace?.TryGetContainedDocument(document.Id);
        }
    }
}
