// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Host;

namespace StarkPlatform.VisualStudio.LanguageServices.Utilities
{
    internal interface ICompilationOptionsChangingService : ILanguageService
    {
        bool CanApplyChange(CompilationOptions oldOptions, CompilationOptions newOptions);

        void Apply(CompilationOptions options, ProjectPropertyStorage storage);
    }
}
