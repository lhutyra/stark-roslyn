// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using StarkPlatform.CodeAnalysis.FindUsages;
using StarkPlatform.CodeAnalysis.Host;

namespace StarkPlatform.CodeAnalysis.Editor.FindUsages
{
    internal interface IFindUsagesService : ILanguageService
    {
        /// <summary>
        /// Finds the references for the symbol at the specific position in the document,
        /// pushing the results into the context instance.
        /// </summary>
        Task FindReferencesAsync(Document document, int position, IFindUsagesContext context);

        /// <summary>
        /// Finds the implementations for the symbol at the specific position in the document,
        /// pushing the results into the context instance.
        /// </summary>
        Task FindImplementationsAsync(Document document, int position, IFindUsagesContext context);
    }
}
