﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis.Host.Mef;
using System.Collections.Generic;

namespace StarkPlatform.CodeAnalysis.QuickInfo
{
    internal class QuickInfoProviderMetadata : OrderableLanguageMetadata
    {
        public QuickInfoProviderMetadata(IDictionary<string, object> data)
            : base(data)
        {
        }
    }
}