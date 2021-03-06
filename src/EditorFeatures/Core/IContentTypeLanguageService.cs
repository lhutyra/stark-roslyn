﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis.Host;
using Microsoft.VisualStudio.Utilities;

namespace StarkPlatform.CodeAnalysis.Editor
{
    /// <summary>
    /// Service to provide the default content type for a language.
    /// </summary>
    internal interface IContentTypeLanguageService : ILanguageService
    {
        IContentType GetDefaultContentType();
    }
}
