﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using StarkPlatform.CodeAnalysis.Editor.Shared.Utilities;
using StarkPlatform.CodeAnalysis.Shared.TestHooks;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;

namespace StarkPlatform.CodeAnalysis.Editor.Tagging
{
    internal abstract class AsynchronousTaggerProvider<TTag> : AbstractAsynchronousTaggerProvider<TTag>, ITaggerProvider
        where TTag : ITag
    {
        protected AsynchronousTaggerProvider(
            IThreadingContext threadingContext,
            IAsynchronousOperationListener asyncListener,
            IForegroundNotificationService notificationService)
                : base(threadingContext, asyncListener, notificationService)
        {
        }

        public IAccurateTagger<T> CreateTagger<T>(ITextBuffer subjectBuffer) where T : ITag
        {
            if (subjectBuffer == null)
            {
                throw new ArgumentNullException(nameof(subjectBuffer));
            }

            return this.CreateTaggerWorker<T>(null, subjectBuffer);
        }

        ITagger<T> ITaggerProvider.CreateTagger<T>(ITextBuffer buffer)
        {
            return CreateTagger<T>(buffer);
        }
    }
}
