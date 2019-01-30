// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.LanguageServices.ProjectInfoService;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.ProjectSystem;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.ProjectInfoService
{
    internal sealed class DefaultProjectInfoService : IProjectInfoService
    {
        public bool GeneratedTypesMustBePublic(Project project)
        {
            var workspace = project.Solution.Workspace as VisualStudioWorkspaceImpl;
            if (workspace == null)
            {
                return false;
            }

            // TODO: reimplement

            return false;
        }
    }
}
