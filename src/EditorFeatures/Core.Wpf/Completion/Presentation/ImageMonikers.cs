// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using StarkPlatform.CodeAnalysis.Editor.Wpf;
using Microsoft.VisualStudio.Imaging.Interop;

namespace StarkPlatform.CodeAnalysis.Editor.Implementation.IntelliSense.Completion.Presentation
{
    internal static class ImageMonikers
    {
        public static ImageMoniker GetFirstImageMoniker(ImmutableArray<string> tags)
        {
            return tags.GetFirstGlyph().GetImageMoniker();
        }
    }
}
