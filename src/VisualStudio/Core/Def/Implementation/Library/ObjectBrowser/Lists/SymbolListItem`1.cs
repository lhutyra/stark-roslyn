// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.Library.ObjectBrowser.Lists
{
    internal abstract class SymbolListItem<TSymbol> : SymbolListItem
        where TSymbol : ISymbol
    {
        protected SymbolListItem(ProjectId projectId, TSymbol symbol, string displayText, string fullNameText, string searchText, bool isHidden)
           : base(projectId, symbol, displayText, fullNameText, searchText, isHidden)
        {
        }

        public TSymbol ResolveTypedSymbol(Compilation compilation)
        {
            return (TSymbol)ResolveSymbol(compilation);
        }
    }
}
