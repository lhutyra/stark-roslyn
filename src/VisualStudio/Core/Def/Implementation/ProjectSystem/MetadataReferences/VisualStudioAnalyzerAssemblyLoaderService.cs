// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Composition;
using System.IO;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Host;
using StarkPlatform.CodeAnalysis.Host.Mef;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.ProjectSystem
{
    [ExportWorkspaceService(typeof(IAnalyzerService), ServiceLayer.Host), Shared]
    internal sealed class VsAnalyzerAssemblyLoaderService : IAnalyzerService
    {
        private readonly ShadowCopyAnalyzerAssemblyLoader _loader = new ShadowCopyAnalyzerAssemblyLoader(Path.Combine(Path.GetTempPath(), "VS", "AnalyzerAssemblyLoader"));

        public IAnalyzerAssemblyLoader GetLoader()
        {
            return _loader;
        }
    }
}
