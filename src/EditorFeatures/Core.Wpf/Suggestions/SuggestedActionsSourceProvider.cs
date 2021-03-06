﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using StarkPlatform.CodeAnalysis.CodeFixes;
using StarkPlatform.CodeAnalysis.CodeRefactorings;
using StarkPlatform.CodeAnalysis.Diagnostics;
using StarkPlatform.CodeAnalysis.Editor.Host;
using StarkPlatform.CodeAnalysis.Editor.Shared.Utilities;
using StarkPlatform.CodeAnalysis.Editor.Tags;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.CodeAnalysis.Shared.TestHooks;
using StarkPlatform.CodeAnalysis.Shared.Utilities;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Roslyn.Utilities;

namespace StarkPlatform.CodeAnalysis.Editor.Implementation.Suggestions
{
    [Export(typeof(ISuggestedActionsSourceProvider))]
    [Export(typeof(SuggestedActionsSourceProvider))]
    [Microsoft.VisualStudio.Utilities.ContentType(ContentTypeNames.StarkRoslynContentType)]
    [Microsoft.VisualStudio.Utilities.Name("Roslyn Code Fix")]
    [Microsoft.VisualStudio.Utilities.Order]
    internal partial class SuggestedActionsSourceProvider : ISuggestedActionsSourceProvider
    {
        private static readonly Guid s_CSharpSourceGuid = new Guid("b967fea8-e2c3-4984-87d4-71a38f49e16a");
        private static readonly Guid s_xamlSourceGuid = new Guid("a0572245-2eab-4c39-9f61-06a6d8c5ddda");

        private const int InvalidSolutionVersion = -1;

        private readonly IThreadingContext _threadingContext;
        private readonly ICodeRefactoringService _codeRefactoringService;
        private readonly IDiagnosticAnalyzerService _diagnosticService;
        private readonly ICodeFixService _codeFixService;
        private readonly ISuggestedActionCategoryRegistryService _suggestedActionCategoryRegistry;

        public readonly ICodeActionEditHandlerService EditHandler;
        public readonly IAsynchronousOperationListener OperationListener;
        public readonly IWaitIndicator WaitIndicator;
        public readonly ImmutableArray<Lazy<ISuggestedActionCallback>> ActionCallbacks;

        public readonly ImmutableArray<Lazy<IImageMonikerService, OrderableMetadata>> ImageMonikerServices;

        [ImportingConstructor]
        [Obsolete(MefConstruction.ImportingConstructorMessage, error: true)]
        public SuggestedActionsSourceProvider(
            IThreadingContext threadingContext,
            ICodeRefactoringService codeRefactoringService,
            IDiagnosticAnalyzerService diagnosticService,
            ICodeFixService codeFixService,
            ICodeActionEditHandlerService editHandler,
            IWaitIndicator waitIndicator,
            ISuggestedActionCategoryRegistryService suggestedActionCategoryRegistry,
            IAsynchronousOperationListenerProvider listenerProvider,
            [ImportMany] IEnumerable<Lazy<IImageMonikerService, OrderableMetadata>> imageMonikerServices,
            [ImportMany] IEnumerable<Lazy<ISuggestedActionCallback>> actionCallbacks)
        {
            _threadingContext = threadingContext;
            _codeRefactoringService = codeRefactoringService;
            _diagnosticService = diagnosticService;
            _codeFixService = codeFixService;
            _suggestedActionCategoryRegistry = suggestedActionCategoryRegistry;
            ActionCallbacks = actionCallbacks.ToImmutableArray();
            EditHandler = editHandler;
            WaitIndicator = waitIndicator;
            OperationListener = listenerProvider.GetListener(FeatureAttribute.LightBulb);

            ImageMonikerServices = ExtensionOrderer.Order(imageMonikerServices).ToImmutableArray();
        }

        public ISuggestedActionsSource CreateSuggestedActionsSource(ITextView textView, ITextBuffer textBuffer)
        {
            Contract.ThrowIfNull(textView);
            Contract.ThrowIfNull(textBuffer);

            return new SuggestedActionsSource(_threadingContext, this, textView, textBuffer, _suggestedActionCategoryRegistry);
        }
    }
}
