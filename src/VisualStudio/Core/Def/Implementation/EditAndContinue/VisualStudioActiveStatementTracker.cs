// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using StarkPlatform.CodeAnalysis.EditAndContinue;
using Microsoft.VisualStudio.Debugger.UI.Interfaces;
using StarkPlatform.VisualStudio.LanguageServices.Utilities;
using VsTextSpan = Microsoft.VisualStudio.TextManager.Interop.TextSpan;

namespace StarkPlatform.VisualStudio.LanguageServices.EditAndContinue
{
    //[Export(typeof(IManagedActiveStatementTracker)), Shared]
    //internal sealed class VisualStudioActiveStatementTracker : IManagedActiveStatementTracker
    //{
    //    private readonly IEditAndContinueService _editAndContinueService;

    //    [ImportingConstructor]
    //    public VisualStudioActiveStatementTracker(IEditAndContinueService editAndContinueService)
    //    {
    //        _editAndContinueService = editAndContinueService;
    //    }

    //    public async Task<VsTextSpan?> GetCurrentActiveStatementPositionAsync(Guid moduleId, int methodToken, int methodVersion, int ilOffset, CancellationToken cancellationToken)
    //        => (await _editAndContinueService.GetCurrentActiveStatementPositionAsync(new ActiveInstructionId(moduleId, methodToken, methodVersion, ilOffset), cancellationToken).ConfigureAwait(false))?.ToVsTextSpan();

    //    public Task<bool?> IsActiveStatementInExceptionRegionAsync(Guid moduleId, int methodToken, int methodVersion, int ilOffset, CancellationToken cancellationToken)
    //        => _editAndContinueService.IsActiveStatementInExceptionRegionAsync(new ActiveInstructionId(moduleId, methodToken, methodVersion, ilOffset), cancellationToken);
    //}
}
