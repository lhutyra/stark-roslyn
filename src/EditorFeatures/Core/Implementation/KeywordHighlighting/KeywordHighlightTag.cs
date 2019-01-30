// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis.Editor.Shared.Tagging;

namespace StarkPlatform.CodeAnalysis.Editor.Implementation.Highlighting
{
    internal class KeywordHighlightTag : NavigableHighlightTag
    {
        internal const string TagId = "MarkerFormatDefinition/HighlightedReference";

        public static readonly KeywordHighlightTag Instance = new KeywordHighlightTag();

        private KeywordHighlightTag()
            : base(TagId)
        {
        }
    }
}
