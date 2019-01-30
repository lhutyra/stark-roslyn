// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Host;
using StarkPlatform.CodeAnalysis.Text;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.Progression
{
    internal interface IProgressionLanguageService : ILanguageService
    {
        IEnumerable<SyntaxNode> GetTopLevelNodesFromDocument(SyntaxNode root, CancellationToken cancellationToken);
        string GetDescriptionForSymbol(ISymbol symbol, bool includeContainingSymbol);
        string GetLabelForSymbol(ISymbol symbol, bool includeContainingSymbol);
    }
}
