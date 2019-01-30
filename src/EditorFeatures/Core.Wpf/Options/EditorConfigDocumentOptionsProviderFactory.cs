﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Composition;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.CodeAnalysis.Options;
using StarkPlatform.CodeAnalysis.Shared.TestHooks;
using Microsoft.VisualStudio.CodingConventions;

namespace StarkPlatform.CodeAnalysis.Editor.Options
{
    [Export(typeof(IDocumentOptionsProviderFactory)), Shared]
    class EditorConfigDocumentOptionsProviderFactory : IDocumentOptionsProviderFactory
    {
        private readonly ICodingConventionsManager _codingConventionsManager;
        private readonly IAsynchronousOperationListenerProvider _asynchronousOperationListenerProvider;

        [ImportingConstructor]
        [Obsolete(MefConstruction.ImportingConstructorMessage, error: true)]
        public EditorConfigDocumentOptionsProviderFactory(
            ICodingConventionsManager codingConventionsManager,
            IAsynchronousOperationListenerProvider asynchronousOperationListenerProvider)
        {
            _codingConventionsManager = codingConventionsManager;
            _asynchronousOperationListenerProvider = asynchronousOperationListenerProvider;
        }

        public IDocumentOptionsProvider Create(Workspace workspace)
        {
            return new EditorConfigDocumentOptionsProvider(workspace, _codingConventionsManager, _asynchronousOperationListenerProvider);
        }
    }
}
