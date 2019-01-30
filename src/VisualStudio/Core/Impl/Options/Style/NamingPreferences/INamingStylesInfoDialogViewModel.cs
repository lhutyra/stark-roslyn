﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.Options.Style.NamingPreferences
{
    internal interface INamingStylesInfoDialogViewModel
    {
        string ItemName { get; set; }
        bool CanBeDeleted { get; set; }
    }
}
