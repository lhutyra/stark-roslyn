// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using StarkPlatform.CodeAnalysis.Editor.Implementation.ExtractInterface;
using StarkPlatform.CodeAnalysis.Editor.Shared.Utilities;
using Microsoft.VisualStudio.Utilities;
using VSCommanding = Microsoft.VisualStudio.Commanding;

namespace StarkPlatform.CodeAnalysis.Editor.CSharp.ExtractInterface
{
    [Export(typeof(VSCommanding.ICommandHandler))]
    [ContentType(ContentTypeNames.StarkContentType)]
    [Name(PredefinedCommandHandlerNames.ExtractInterface)]
    internal class ExtractInterfaceCommandHandler : AbstractExtractInterfaceCommandHandler
    {
        [ImportingConstructor]
        public ExtractInterfaceCommandHandler(IThreadingContext threadingContext)
            : base(threadingContext)
        {
        }
    }
}
