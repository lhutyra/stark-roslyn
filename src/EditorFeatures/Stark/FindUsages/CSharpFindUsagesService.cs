// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Composition;
using StarkPlatform.CodeAnalysis.Editor.FindUsages;
using StarkPlatform.CodeAnalysis.Editor.Shared.Utilities;
using StarkPlatform.CodeAnalysis.Host.Mef;

namespace StarkPlatform.CodeAnalysis.Editor.CSharp.FindUsages
{
    [ExportLanguageService(typeof(IFindUsagesService), LanguageNames.Stark), Shared]
    internal class CSharpFindUsagesService : AbstractFindUsagesService
    {
        [ImportingConstructor]
        [Obsolete(MefConstruction.ImportingConstructorMessage, error: true)]
        public CSharpFindUsagesService(IThreadingContext threadingContext)
            : base(threadingContext)
        {
        }
    }
}
