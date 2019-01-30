// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Composition;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.ProjectSystem;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.ProjectSystemShim
{
    [ExportLanguageService(typeof(IEntryPointFinderService), LanguageNames.Stark), Shared]
    internal class CSharpEntryPointFinderService : IEntryPointFinderService
    {
        public IEnumerable<INamedTypeSymbol> FindEntryPoints(INamespaceSymbol symbol, bool findFormsOnly)
        {
            return EntryPointFinder.FindEntryPoints(symbol);
        }
    }
}
