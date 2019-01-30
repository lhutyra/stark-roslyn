// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis.Options;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation
{
    internal static class VisualStudioNavigationOptions
    {
        public static readonly PerLanguageOption<bool> NavigateToObjectBrowser = new PerLanguageOption<bool>(nameof(VisualStudioNavigationOptions), nameof(NavigateToObjectBrowser), defaultValue: false,
            storageLocations: new RoamingProfileStorageLocation("TextEditor.%LANGUAGE%.Specific.NavigateToObjectBrowser"));
    }
}
