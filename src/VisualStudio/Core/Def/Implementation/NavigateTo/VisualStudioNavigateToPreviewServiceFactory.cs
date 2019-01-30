// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Composition;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Editor.Implementation.NavigateTo;
using StarkPlatform.CodeAnalysis.Host;
using StarkPlatform.CodeAnalysis.Host.Mef;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.NavigateTo
{
    [ExportWorkspaceServiceFactory(typeof(INavigateToPreviewService), ServiceLayer.Host), Shared]
    internal sealed class VisualStudioNavigateToPreviewServiceFactory : IWorkspaceServiceFactory
    {
        private Lazy<INavigateToPreviewService> _singleton =
            new Lazy<INavigateToPreviewService>(() => new VisualStudioNavigateToPreviewService());

        public IWorkspaceService CreateService(HostWorkspaceServices workspaceServices)
        {
            return _singleton.Value;
        }
    }
}
