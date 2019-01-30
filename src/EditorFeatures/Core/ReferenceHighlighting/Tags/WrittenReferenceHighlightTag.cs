// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis.Editor.Shared.Tagging;

namespace StarkPlatform.CodeAnalysis.Editor.ReferenceHighlighting
{
    internal class WrittenReferenceHighlightTag : NavigableHighlightTag
    {
        internal const string TagId = "MarkerFormatDefinition/HighlightedWrittenReference";

        public static readonly WrittenReferenceHighlightTag Instance = new WrittenReferenceHighlightTag();

        private WrittenReferenceHighlightTag()
            : base(TagId)
        {
        }
    }
}
