// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using StarkPlatform.CodeAnalysis.Features.EmbeddedLanguages;

namespace StarkPlatform.CodeAnalysis.Editor.EmbeddedLanguages
{
    /// <summary>
    /// Service that returns all the embedded languages supported.  Each embedded language can expose
    /// individual language services through the <see cref="IEmbeddedLanguageEditorFeatures"/> interface.
    /// </summary>
    internal interface IEmbeddedLanguageEditorFeaturesProvider : IEmbeddedLanguageFeaturesProvider
    {
        new ImmutableArray<IEmbeddedLanguageEditorFeatures> Languages { get; }
    }
}
