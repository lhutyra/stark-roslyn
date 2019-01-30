// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation
{
    internal interface IBindingRedirectionService
    {
        AssemblyIdentity ApplyBindingRedirects(AssemblyIdentity originalIdentity);
    }
}
