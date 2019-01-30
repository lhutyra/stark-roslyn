// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis.Editor;
using StarkPlatform.CodeAnalysis.Editor.CSharp.Formatting.Indentation;
using StarkPlatform.CodeAnalysis.Editor.Wrapping.SeparatedSyntaxList;

namespace StarkPlatform.CodeAnalysis.Stark.Editor.Wrapping.SeparatedSyntaxList
{
    internal abstract class AbstractCSharpSeparatedSyntaxListWrapper<TListSyntax, TListItemSyntax>
        : AbstractSeparatedSyntaxListWrapper<TListSyntax, TListItemSyntax>
        where TListSyntax : SyntaxNode
        where TListItemSyntax : SyntaxNode
    {
        // In our scenario we want to control all formatting ourselves. So tell the indenter to not
        // depend on a formatter being available so it does all the work to figure out indentation
        // itself.
        protected override IBlankLineIndentationService GetIndentationService()
            => new CSharpIndentationService();
    }
}
