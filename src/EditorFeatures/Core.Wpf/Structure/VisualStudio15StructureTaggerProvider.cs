﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using StarkPlatform.CodeAnalysis.Editor.Implementation.Structure;
using StarkPlatform.CodeAnalysis.Editor.Shared.Utilities;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.CodeAnalysis.Shared.TestHooks;
using StarkPlatform.CodeAnalysis.Structure;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Projection;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

#pragma warning disable CS0618 // Type or member is obsolete
namespace StarkPlatform.CodeAnalysis.Editor.Structure
{
    [Export(typeof(ITaggerProvider))]
    [Export(typeof(VisualStudio15StructureTaggerProvider))]
    [TagType(typeof(IBlockTag))]
    [ContentType(ContentTypeNames.StarkRoslynContentType)]
    internal partial class VisualStudio15StructureTaggerProvider :
        AbstractStructureTaggerProvider<IBlockTag>
    {
        [ImportingConstructor]
        [Obsolete(MefConstruction.ImportingConstructorMessage, error: true)]
        public VisualStudio15StructureTaggerProvider(
            IThreadingContext threadingContext,
            IForegroundNotificationService notificationService,
            ITextEditorFactoryService textEditorFactoryService,
            IEditorOptionsFactoryService editorOptionsFactoryService,
            IProjectionBufferFactoryService projectionBufferFactoryService,
            IAsynchronousOperationListenerProvider listenerProvider)
                : base(threadingContext, notificationService, textEditorFactoryService, editorOptionsFactoryService, projectionBufferFactoryService, listenerProvider)
        {
        }

        protected override IBlockTag CreateTag(
            IBlockTag parentTag, ITextSnapshot snapshot, BlockSpan region)
        {
            return new RoslynBlockTag(
                ThreadingContext,
                this.TextEditorFactoryService,
                this.ProjectionBufferFactoryService,
                this.EditorOptionsFactoryService,
                parentTag, snapshot, region);
        }
    }
}

#pragma warning restore CS0618 // Type or member is obsolete
