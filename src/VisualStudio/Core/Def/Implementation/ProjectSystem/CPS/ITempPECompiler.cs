﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StarkPlatform.VisualStudio.LanguageServices.ProjectSystem
{
    /// <summary>
    /// Provides TempPE compiler access
    /// </summary>
    /// <remarks>
    /// This is used by the project system to enable designer support in Visual Studio
    /// </remarks>
    internal interface ITempPECompiler
    {
        /// <summary>
        /// Compiles specific files into the TempPE DLL to provide designer support
        /// </summary>
        /// <param name="context">The project context</param>
        /// <param name="outputFileName">The binary output path</param>
        /// <param name="filesToInclude">Set of file paths from the project that should be included in the output. Should use StringComparer.OrdinalIgnoreCase to avoid file system issues.</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns><see langword="true" /> if the compilation was successful</returns>
        /// <exception cref="System.IO.IOException">If the <paramref name="outputFileName"/> could not be written</exception>
        Task<bool> CompileAsync(IWorkspaceProjectContext context, string outputFileName, ISet<string> filesToInclude, CancellationToken cancellationToken);
    }
}
