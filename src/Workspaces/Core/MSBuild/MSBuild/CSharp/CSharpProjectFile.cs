// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using StarkPlatform.CodeAnalysis.MSBuild;
using StarkPlatform.CodeAnalysis.MSBuild.Build;
using StarkPlatform.CodeAnalysis.MSBuild.Logging;
using MSB = Microsoft.Build;

namespace StarkPlatform.CodeAnalysis.Stark
{
    internal class CSharpProjectFile : ProjectFile
    {
        public CSharpProjectFile(CSharpProjectFileLoader loader, MSB.Evaluation.Project project, ProjectBuildManager buildManager, DiagnosticLog log)
            : base(loader, project, buildManager, log)
        {
        }

        protected override SourceCodeKind GetSourceCodeKind(string documentFileName)
            => SourceCodeKind.Regular;

        public override string GetDocumentExtension(SourceCodeKind sourceCodeKind)
            => ".cs";

        protected override IEnumerable<MSB.Framework.ITaskItem> GetCompilerCommandLineArgs(MSB.Execution.ProjectInstance executedProject)
            => executedProject.GetItems(ItemNames.CscCommandLineArgs);

        protected override ImmutableArray<string> ReadCommandLineArgs(MSB.Execution.ProjectInstance project)
            => CSharpCommandLineArgumentReader.Read(project);
    }
}
