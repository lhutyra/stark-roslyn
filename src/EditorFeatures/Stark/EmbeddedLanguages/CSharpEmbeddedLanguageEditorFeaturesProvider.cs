// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Composition;
using StarkPlatform.CodeAnalysis.Stark.EmbeddedLanguages.LanguageServices;
using StarkPlatform.CodeAnalysis.Editor.EmbeddedLanguages;
using StarkPlatform.CodeAnalysis.Features.EmbeddedLanguages;
using StarkPlatform.CodeAnalysis.Host.Mef;

namespace StarkPlatform.CodeAnalysis.Stark.Editor.EmbeddedLanguages
{
    [ExportLanguageService(typeof(IEmbeddedLanguageEditorFeaturesProvider), LanguageNames.Stark), Shared]
    internal class CSharpEmbeddedLanguageEditorFeaturesProvider : AbstractEmbeddedLanguageEditorFeaturesProvider
    {
        public static IEmbeddedLanguageFeaturesProvider Instance = new CSharpEmbeddedLanguageEditorFeaturesProvider();

        public CSharpEmbeddedLanguageEditorFeaturesProvider() : base(CSharpEmbeddedLanguagesProvider.Info)
        {
        }
    }
}
