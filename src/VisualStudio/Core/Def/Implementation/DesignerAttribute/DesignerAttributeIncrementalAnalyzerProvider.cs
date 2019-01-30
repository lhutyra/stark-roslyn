// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Composition;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Editor;
using StarkPlatform.CodeAnalysis.Editor.Shared.Utilities;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.CodeAnalysis.Shared.TestHooks;
using StarkPlatform.CodeAnalysis.SolutionCrawler;
using Microsoft.VisualStudio.Shell;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.DesignerAttribute
{
    [ExportIncrementalAnalyzerProvider(Name, new[] { WorkspaceKind.Host }), Shared]
    internal class DesignerAttributeIncrementalAnalyzerProvider : IIncrementalAnalyzerProvider
    {
        public const string Name = nameof(DesignerAttributeIncrementalAnalyzerProvider);

        private readonly IThreadingContext _threadingContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly IForegroundNotificationService _notificationService;
        private readonly IAsynchronousOperationListenerProvider _listenerProvider;

        [ImportingConstructor]
        [Obsolete(MefConstruction.ImportingConstructorMessage, error: true)]
        public DesignerAttributeIncrementalAnalyzerProvider(
            IThreadingContext threadingContext,
            SVsServiceProvider serviceProvider,
            IForegroundNotificationService notificationService,
            IAsynchronousOperationListenerProvider listenerProvider)
        {
            _threadingContext = threadingContext;
            _serviceProvider = serviceProvider;
            _notificationService = notificationService;
            _listenerProvider = listenerProvider;
        }

        public IIncrementalAnalyzer CreateIncrementalAnalyzer(CodeAnalysis.Workspace workspace)
        {
            return new DesignerAttributeIncrementalAnalyzer(_threadingContext, _serviceProvider, _notificationService, _listenerProvider);
        }
    }
}
