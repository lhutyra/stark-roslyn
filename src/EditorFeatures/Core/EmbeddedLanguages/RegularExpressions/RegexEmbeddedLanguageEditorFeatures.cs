// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis.Editor;
using StarkPlatform.CodeAnalysis.Editor.EmbeddedLanguages;
using StarkPlatform.CodeAnalysis.EmbeddedLanguages.LanguageServices;

namespace StarkPlatform.CodeAnalysis.Features.EmbeddedLanguages.RegularExpressions
{
    internal class RegexEmbeddedLanguageEditorFeatures : RegexEmbeddedLanguageFeatures, IEmbeddedLanguageEditorFeatures
    {
        public IBraceMatcher BraceMatcher { get; }

        public RegexEmbeddedLanguageEditorFeatures(EmbeddedLanguageInfo info) : base(info)
        {
            BraceMatcher = new RegexBraceMatcher(this);
        }
    }
}
