﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Composition;
using StarkPlatform.CodeAnalysis.Host;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.CodeAnalysis.LanguageServices;

namespace StarkPlatform.CodeAnalysis.Stark
{
    [ExportLanguageService(typeof(ICompilationFactoryService), LanguageNames.Stark), Shared]
    internal class CSharpCompilationFactoryService : ICompilationFactoryService
    {
        private static readonly CSharpCompilationOptions s_defaultOptions = new CSharpCompilationOptions(OutputKind.ConsoleApplication, concurrentBuild: false);

        Compilation ICompilationFactoryService.CreateCompilation(string assemblyName, CompilationOptions options)
        {
            return CSharpCompilation.Create(
                assemblyName,
                options: (CSharpCompilationOptions)options ?? s_defaultOptions);
        }

        Compilation ICompilationFactoryService.CreateSubmissionCompilation(string assemblyName, CompilationOptions options, Type hostObjectType)
        {
            return CSharpCompilation.CreateScriptCompilation(
                assemblyName,
                options: (CSharpCompilationOptions)options,
                previousScriptCompilation: null,
                globalsType: hostObjectType);
        }

        Compilation ICompilationFactoryService.GetCompilationFromCompilationReference(MetadataReference reference)
        {
            var compilationRef = reference as CompilationReference;
            return (compilationRef != null) ? compilationRef.Compilation : null;
        }

        bool ICompilationFactoryService.IsCompilationReference(MetadataReference reference)
        {
            return reference is CompilationReference;
        }

        CompilationOptions ICompilationFactoryService.GetDefaultCompilationOptions()
        {
            return s_defaultOptions;
        }
    }
}
