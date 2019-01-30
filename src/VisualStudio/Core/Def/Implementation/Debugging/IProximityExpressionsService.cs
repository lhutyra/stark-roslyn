// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Host;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.Debugging
{
    internal interface IProximityExpressionsService : ILanguageService
    {
        Task<IList<string>> GetProximityExpressionsAsync(Document document, int position, CancellationToken cancellationToken);
        Task<bool> IsValidAsync(Document document, int position, string expressionValue, CancellationToken cancellationToken);
    }
}
