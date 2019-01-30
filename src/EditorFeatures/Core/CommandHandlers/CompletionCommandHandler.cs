// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using StarkPlatform.CodeAnalysis.Editor.Shared.Utilities;
using StarkPlatform.CodeAnalysis.Host.Mef;
using Microsoft.VisualStudio.Utilities;
using VSCommanding = Microsoft.VisualStudio.Commanding;

namespace StarkPlatform.CodeAnalysis.Editor.CommandHandlers
{
    [Export]
    [Export(typeof(VSCommanding.ICommandHandler))]
    [ContentType(ContentTypeNames.StarkRoslynContentType)]
    [Name(PredefinedCommandHandlerNames.Completion)]
    [Order(After = PredefinedCommandHandlerNames.SignatureHelp,
           Before = PredefinedCommandHandlerNames.DocumentationComments)]
    internal sealed class CompletionCommandHandler : AbstractCompletionCommandHandler
    {
        [ImportingConstructor]
        [Obsolete(MefConstruction.ImportingConstructorMessage, error: true)]
        public CompletionCommandHandler(IThreadingContext threadingContext, IAsyncCompletionService completionService)
            : base(threadingContext, completionService)
        {
        }
    }
}
