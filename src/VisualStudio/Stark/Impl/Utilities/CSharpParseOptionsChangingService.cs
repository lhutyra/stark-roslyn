// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Composition;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Stark;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.VisualStudio.LanguageServices.Utilities;
using VSLangProj80;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.Utilities
{
    [ExportLanguageService(typeof(IParseOptionsChangingService), LanguageNames.Stark), Shared]
    internal class CSharpParseOptionsChangingService : IParseOptionsChangingService
    {
        public bool CanApplyChange(ParseOptions oldOptions, ParseOptions newOptions)
        {
            var oldCSharpOptions = (CSharpParseOptions)oldOptions;
            var newCSharpOptions = (CSharpParseOptions)newOptions;

            // Currently, only changes to the LanguageVersion of parse options are supported.
            return oldCSharpOptions.WithLanguageVersion(newCSharpOptions.SpecifiedLanguageVersion) == newOptions;
        }

        public void Apply(ParseOptions options, ProjectPropertyStorage storage)
        {
            var csharpOptions = (CSharpParseOptions)options;

            storage.SetProperty("LangVersion", nameof(CSharpProjectConfigurationProperties3.LanguageVersion),
                LanguageVersionFacts.ToDisplayString(csharpOptions.SpecifiedLanguageVersion));
        }
    }
}
