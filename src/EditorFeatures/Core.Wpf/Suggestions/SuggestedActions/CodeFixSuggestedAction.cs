// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis.CodeActions;
using StarkPlatform.CodeAnalysis.CodeFixes;
using StarkPlatform.CodeAnalysis.Diagnostics;
using StarkPlatform.CodeAnalysis.Editor.Shared.Extensions;
using StarkPlatform.CodeAnalysis.Editor.Shared.Utilities;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;

namespace StarkPlatform.CodeAnalysis.Editor.Implementation.Suggestions
{
    /// <summary>
    /// Represents light bulb menu item for code fixes.
    /// </summary>
    internal sealed class CodeFixSuggestedAction : SuggestedActionWithNestedFlavors, ITelemetryDiagnosticID<string>
    {
        private readonly CodeFix _fix;

        public CodeFixSuggestedAction(
            IThreadingContext threadingContext,
            SuggestedActionsSourceProvider sourceProvider,
            Workspace workspace,
            ITextBuffer subjectBuffer,
            CodeFix fix,
            object provider,
            CodeAction action,
            SuggestedActionSet fixAllFlavors)
            : base(threadingContext, sourceProvider, workspace, subjectBuffer,
                   provider, action, fixAllFlavors)
        {
            _fix = fix;
        }

        public string GetDiagnosticID()
        {
            return _fix.PrimaryDiagnostic.GetTelemetryDiagnosticID();
        }

        protected override DiagnosticData GetDiagnostic()
        {
            return _fix.GetPrimaryDiagnosticData();
        }
    }
}
