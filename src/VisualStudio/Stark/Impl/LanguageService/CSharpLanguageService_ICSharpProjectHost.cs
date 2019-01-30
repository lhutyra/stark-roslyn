// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.IO;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Editor.Shared.Utilities;
using StarkPlatform.CodeAnalysis.Host;
using StarkPlatform.VisualStudio.LanguageServices.CSharp.ProjectSystemShim;
using StarkPlatform.VisualStudio.LanguageServices.CSharp.ProjectSystemShim.Interop;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.TaskList;
using Microsoft.VisualStudio.Shell.Interop;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.LanguageService
{
    internal partial class StarkLanguageService : ICSharpProjectHost
    {
        public void BindToProject(ICSharpProjectRoot projectRoot, IVsHierarchy hierarchy)
        {
            var projectName = Path.GetFileName(projectRoot.GetFullProjectName()); // GetFullProjectName returns the path to the project file w/o the extension?

            var project = new CSharpProjectShim(
                projectRoot,
                projectName,
                hierarchy,
                this.SystemServiceProvider,
                this.Package.ComponentModel.GetService<IThreadingContext>(),
                this.HostDiagnosticUpdateSource,
                this.Workspace.Services.GetLanguageServices(LanguageNames.Stark).GetService<ICommandLineParserService>());

            projectRoot.SetProjectSite(project);
        }
    }
}
