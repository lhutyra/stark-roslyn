// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Editor.Implementation.Debugging;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.Debugging;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.Debugging
{
    [ExportLanguageService(typeof(ILanguageDebugInfoService), LanguageNames.Stark), Shared]
    internal partial class CSharpLanguageDebugInfoService : ILanguageDebugInfoService
    {
        public Task<DebugLocationInfo> GetLocationInfoAsync(Document document, int position, CancellationToken cancellationToken)
        {
            return LocationInfoGetter.GetInfoAsync(document, position, cancellationToken);
        }

        public Task<DebugDataTipInfo> GetDataTipInfoAsync(
            Document document, int position, CancellationToken cancellationToken)
        {
            return DataTipInfoGetter.GetInfoAsync(document, position, cancellationToken);
        }
    }
}
