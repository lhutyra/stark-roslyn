// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using StarkPlatform.CodeAnalysis.Editor.Implementation.Adornments;
using StarkPlatform.CodeAnalysis.Editor.Shared.Utilities;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.CodeAnalysis.Shared.TestHooks;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace StarkPlatform.CodeAnalysis.Editor.Implementation.LineSeparators
{
    /// <summary>
    /// This factory is called to create the view service that will manage line separators.
    /// </summary>
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType(ContentTypeNames.StarkRoslynContentType)]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal class LineSeparatorAdornmentManagerProvider :
        AbstractAdornmentManagerProvider<LineSeparatorTag>
    {
        private const string LayerName = "RoslynLineSeparator";

        [Export]
        [Name(LayerName)]
        [ContentType(ContentTypeNames.StarkRoslynContentType)]
        [Order(After = PredefinedAdornmentLayers.Selection, Before = PredefinedAdornmentLayers.Squiggle)]
#pragma warning disable 0169
        private readonly AdornmentLayerDefinition _lineSeparatorLayer;
#pragma warning restore 0169

        [ImportingConstructor]
        [Obsolete(MefConstruction.ImportingConstructorMessage, error: true)]
        public LineSeparatorAdornmentManagerProvider(
            IThreadingContext threadingContext,
            IViewTagAggregatorFactoryService tagAggregatorFactoryService,
            IAsynchronousOperationListenerProvider listenerProvider)
            : base(threadingContext, tagAggregatorFactoryService, listenerProvider)
        {
        }

        protected override string FeatureAttributeName => FeatureAttribute.LineSeparators;
        protected override string AdornmentLayerName => LayerName;
    }
}
