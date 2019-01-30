// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis.Editor.Shared.Tagging;

namespace StarkPlatform.CodeAnalysis.Editor.ReferenceHighlighting
{
    internal class DefinitionHighlightTag : NavigableHighlightTag
    {
        public const string TagId = "MarkerFormatDefinition/HighlightedDefinition";

        public static readonly DefinitionHighlightTag Instance = new DefinitionHighlightTag();

        private DefinitionHighlightTag()
            : base(TagId)
        {
        }
    }
}
