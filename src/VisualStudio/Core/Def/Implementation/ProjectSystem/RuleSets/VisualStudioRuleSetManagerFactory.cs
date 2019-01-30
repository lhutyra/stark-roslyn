// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Composition;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Editor;
using StarkPlatform.CodeAnalysis.Host;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.CodeAnalysis.Shared.TestHooks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.ProjectSystem
{
    [ExportWorkspaceServiceFactory(typeof(VisualStudioRuleSetManager), ServiceLayer.Host), Shared]
    internal sealed class VisualStudioRuleSetManagerFactory : IWorkspaceServiceFactory
    {
        private readonly FileChangeWatcherProvider _fileChangeWatcherProvider;
        private readonly IForegroundNotificationService _foregroundNotificationService;
        private readonly IAsynchronousOperationListener _listener;

        [ImportingConstructor]
        public VisualStudioRuleSetManagerFactory(
            FileChangeWatcherProvider fileChangeWatcherProvider,
            IForegroundNotificationService foregroundNotificationService,
            IAsynchronousOperationListenerProvider listenerProvider)
        {
            _fileChangeWatcherProvider = fileChangeWatcherProvider;
            _foregroundNotificationService = foregroundNotificationService;
            _listener = listenerProvider.GetListener(FeatureAttribute.RuleSetEditor);
        }

        public IWorkspaceService CreateService(HostWorkspaceServices workspaceServices)
        {
            return new VisualStudioRuleSetManager(_fileChangeWatcherProvider.Watcher, _foregroundNotificationService, _listener);
        }
    }
}
