// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Text;
using Microsoft.VisualStudio.GraphModel;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.Progression
{
    internal interface IGraphQuery
    {
        Task<GraphBuilder> GetGraphAsync(Solution solution, IGraphContext context, CancellationToken cancellationToken);
    }
}
