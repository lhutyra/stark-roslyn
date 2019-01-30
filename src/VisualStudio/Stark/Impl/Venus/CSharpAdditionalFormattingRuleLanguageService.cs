// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Composition;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Formatting.Rules;
using StarkPlatform.CodeAnalysis.Host;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.VisualStudio.LanguageServices.CSharp.Utilities;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.Venus;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.Venus
{
    [ExportLanguageService(typeof(IAdditionalFormattingRuleLanguageService), LanguageNames.Stark), Shared]
    internal class CSharpAdditionalFormattingRuleLanguageService : IAdditionalFormattingRuleLanguageService
    {
        public IFormattingRule GetAdditionalCodeGenerationRule()
        {
            return new BlankLineInGeneratedMethodFormattingRule();
        }
    }
}
