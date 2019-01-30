// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using StarkPlatform.CodeAnalysis.Host;
using StarkPlatform.CodeAnalysis.MSBuild.Build;

namespace StarkPlatform.CodeAnalysis.MSBuild
{
    internal interface IProjectFileLoader : ILanguageService
    {
        string Language { get; }
        Task<IProjectFile> LoadProjectFileAsync(
            string path,
            ProjectBuildManager buildManager,
            CancellationToken cancellationToken);
    }
}
