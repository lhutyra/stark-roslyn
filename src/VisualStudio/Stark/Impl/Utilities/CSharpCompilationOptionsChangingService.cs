// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Composition;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Stark;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.VisualStudio.LanguageServices.Utilities;
using VSLangProj80;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.Utilities
{
    [ExportLanguageService(typeof(ICompilationOptionsChangingService), LanguageNames.Stark), Shared]
    internal class CSharpCompilationOptionsChangingService : ICompilationOptionsChangingService
    {
        public bool CanApplyChange(CompilationOptions oldOptions, CompilationOptions newOptions)
        {
            var oldCSharpOptions = (CSharpCompilationOptions)oldOptions;
            var newCSharpOptions = (CSharpCompilationOptions)newOptions;

            // Currently, only changes to AllowUnsafe of compilation options are supported.
            return oldCSharpOptions.WithAllowUnsafe(newCSharpOptions.AllowUnsafe) == newOptions;
        }

        public void Apply(CompilationOptions options, ProjectPropertyStorage storage)
        {
            var csharpOptions = (CSharpCompilationOptions)options;

            storage.SetProperty("AllowUnsafeBlocks", nameof(ProjectConfigurationProperties3.AllowUnsafeBlocks),
                csharpOptions.AllowUnsafe);
        }
    }
}
