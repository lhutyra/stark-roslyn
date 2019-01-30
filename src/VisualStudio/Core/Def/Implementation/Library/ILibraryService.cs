// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis.Host;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.Library.VsNavInfo;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.Library
{
    internal interface ILibraryService : ILanguageService
    {
        NavInfoFactory NavInfoFactory { get; }
    }
}
