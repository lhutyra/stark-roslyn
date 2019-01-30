// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using Microsoft.VisualStudio.Text.Tagging;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.Preview
{
    internal class HighlightTag : TextMarkerTag
    {
        public HighlightTag() : base("blue")
        {
        }
    }
}
