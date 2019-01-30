// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Composition;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Editor;
using StarkPlatform.CodeAnalysis.Host;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.VisualStudio.LanguageServices.Implementation;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.CodeModel;
using Microsoft.VisualStudio.Text.Editor;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.CodeModel
{
    [ExportLanguageServiceFactory(typeof(ICodeModelNavigationPointService), LanguageNames.Stark), Shared]
    internal partial class CSharpCodeModelNavigationPointServiceFactory : ILanguageServiceFactory
    {
        public ILanguageService CreateLanguageService(HostLanguageServices provider)
        {
            // This interface is implemented by the ICodeModelService as well, so just grab the other one and return it
            return provider.GetService<ICodeModelService>();
        }
    }
}
