// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using StarkPlatform.CodeAnalysis.Editor;
using StarkPlatform.CodeAnalysis.Editor.Shared.Utilities;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.Library.ClassView;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Utilities;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.ObjectBrowser
{
    [Export(typeof(Microsoft.VisualStudio.Commanding.ICommandHandler))]
    [ContentType(ContentTypeNames.StarkContentType)]
    [Name(PredefinedCommandHandlerNames.ClassView)]
    internal class CSharpSyncClassViewCommandHandler : AbstractSyncClassViewCommandHandler
    {
        [ImportingConstructor]
        [Obsolete(MefConstruction.ImportingConstructorMessage, error: true)]
        private CSharpSyncClassViewCommandHandler(IThreadingContext threadingContext, SVsServiceProvider serviceProvider)
            : base(threadingContext, serviceProvider)
        {
        }
    }
}
