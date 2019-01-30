// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis.MSBuild;
using StarkPlatform.CodeAnalysis.MSBuild.Build;
using StarkPlatform.CodeAnalysis.MSBuild.Logging;
using MSB = Microsoft.Build;

namespace StarkPlatform.CodeAnalysis.Stark
{
    internal partial class CSharpProjectFileLoader : ProjectFileLoader
    {
        public CSharpProjectFileLoader()
        {
        }

        public override string Language
        {
            get { return LanguageNames.Stark; }
        }

        protected override ProjectFile CreateProjectFile(MSB.Evaluation.Project project, ProjectBuildManager buildManager, DiagnosticLog log)
        {
            return new CSharpProjectFile(this, project, buildManager, log);
        }
    }
}
