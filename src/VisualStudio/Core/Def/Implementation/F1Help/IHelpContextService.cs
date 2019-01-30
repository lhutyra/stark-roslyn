// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Host;
using StarkPlatform.CodeAnalysis.Text;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.F1Help
{
    internal interface IHelpContextService : ILanguageService
    {
        string Language { get; }
        string Product { get; }

        Task<string> GetHelpTermAsync(Document document, TextSpan textSpan, CancellationToken cancellationToken);

        string FormatSymbol(ISymbol symbol);
    }
}
