// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using StarkPlatform.CodeAnalysis.Editor.Shared.Extensions;
using StarkPlatform.CodeAnalysis.Editor.Shared.Options;
using StarkPlatform.CodeAnalysis.Editor.Shared.Utilities;
using StarkPlatform.CodeAnalysis.Shared.TestHooks;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;

namespace StarkPlatform.CodeAnalysis.Editor.Implementation.Adornments
{
    internal abstract class AbstractAdornmentManagerProvider<TTag> :
        IWpfTextViewCreationListener
        where TTag : GraphicsTag
    {
        private readonly IThreadingContext _threadingContext;
        private readonly IViewTagAggregatorFactoryService _tagAggregatorFactoryService;
        private readonly IAsynchronousOperationListener _asyncListener;

        protected AbstractAdornmentManagerProvider(
            IThreadingContext threadingContext,
            IViewTagAggregatorFactoryService tagAggregatorFactoryService,
            IAsynchronousOperationListenerProvider listenerProvider)
        {
            _threadingContext = threadingContext;
            _tagAggregatorFactoryService = tagAggregatorFactoryService;
            _asyncListener = listenerProvider.GetListener(this.FeatureAttributeName);
        }

        protected abstract string FeatureAttributeName { get; }
        protected abstract string AdornmentLayerName { get; }

        public void TextViewCreated(IWpfTextView textView)
        {
            if (textView == null)
            {
                throw new ArgumentNullException(nameof(textView));
            }

            if (!textView.TextBuffer.GetFeatureOnOffOption(EditorComponentOnOffOptions.Adornment))
            {
                return;
            }

            // the manager keeps itself alive by listening to text view events.
            AdornmentManager<TTag>.Create(_threadingContext, textView, _tagAggregatorFactoryService, _asyncListener, AdornmentLayerName);
        }
    }
}
