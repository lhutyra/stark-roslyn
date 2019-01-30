// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarkPlatform.CodeAnalysis.Host;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.CodeAnalysis.LanguageServices.ProjectInfoService;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.ProjectInfoService
{
    [ExportWorkspaceServiceFactory(typeof(IProjectInfoService), ServiceLayer.Editor), Shared]
    internal sealed class DefaultProjectInfoServiceFactory : IWorkspaceServiceFactory
    {
        private Lazy<IProjectInfoService> _singleton =
            new Lazy<IProjectInfoService>(() => new DefaultProjectInfoService());

        public IWorkspaceService CreateService(HostWorkspaceServices workspaceServices)
        {
            return _singleton.Value;
        }
    }
}
