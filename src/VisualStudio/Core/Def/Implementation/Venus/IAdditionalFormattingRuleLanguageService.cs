// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Formatting.Rules;
using StarkPlatform.CodeAnalysis.Host;
using StarkPlatform.CodeAnalysis.Text;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.Venus
{
    internal interface IAdditionalFormattingRuleLanguageService : ILanguageService
    {
        IFormattingRule GetAdditionalCodeGenerationRule();
    }
}
