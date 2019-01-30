// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using StarkPlatform.CodeAnalysis.Editor.Shared.Utilities;
using StarkPlatform.CodeAnalysis.Text;
using Microsoft.VisualStudio.Commanding;
using Microsoft.VisualStudio.Text.Editor.Commanding.Commands;
using Microsoft.VisualStudio.Utilities;
using VSCommanding = Microsoft.VisualStudio.Commanding;

namespace StarkPlatform.CodeAnalysis.Editor.Implementation.RenameTracking
{
    [Export(typeof(VSCommanding.ICommandHandler))]
    [ContentType(ContentTypeNames.StarkRoslynContentType)]
    [Name(PredefinedCommandHandlerNames.RenameTrackingCancellation)]
    [Order(After = PredefinedCommandHandlerNames.SignatureHelp)]
    [Order(After = PredefinedCommandHandlerNames.IntelliSense)]
    [Order(After = PredefinedCommandHandlerNames.AutomaticCompletion)]
    [Order(After = PredefinedCommandHandlerNames.Completion)]
    [Order(After = PredefinedCommandHandlerNames.QuickInfo)]
    [Order(After = PredefinedCommandHandlerNames.EventHookup)]
    internal class RenameTrackingCancellationCommandHandler : VSCommanding.ICommandHandler<EscapeKeyCommandArgs>
    {
        public string DisplayName => EditorFeaturesResources.Rename_Tracking_Cancellation;

        public bool ExecuteCommand(EscapeKeyCommandArgs args, CommandExecutionContext context)
        {
            var document = args.SubjectBuffer.CurrentSnapshot.GetOpenDocumentInCurrentContextWithChanges();

            return document != null &&
                RenameTrackingDismisser.DismissVisibleRenameTracking(document.Project.Solution.Workspace, document.Id);
        }

        public VSCommanding.CommandState GetCommandState(EscapeKeyCommandArgs args)
        {
            return VSCommanding.CommandState.Unspecified;
        }
    }
}
