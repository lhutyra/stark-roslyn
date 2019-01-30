﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis.Diagnostics;
using StarkPlatform.CodeAnalysis.Editor.Tagging;
using StarkPlatform.CodeAnalysis.Text;
using Microsoft.VisualStudio.Text;

namespace StarkPlatform.CodeAnalysis.Editor.Shared.Tagging
{
    internal partial class TaggerEventSources
    {
        private class DiagnosticsChangedEventSource : AbstractTaggerEventSource
        {
            private readonly ITextBuffer _subjectBuffer;
            private readonly IDiagnosticService _service;

            public DiagnosticsChangedEventSource(ITextBuffer subjectBuffer, IDiagnosticService service, TaggerDelay delay)
                : base(delay)
            {
                _subjectBuffer = subjectBuffer;
                _service = service;
            }

            private void OnDiagnosticsUpdated(object sender, DiagnosticsUpdatedArgs e)
            {
                var document = _subjectBuffer.AsTextContainer().GetOpenDocumentInCurrentContext();

                if (document != null && document.Project.Solution.Workspace == e.Workspace && document.Id == e.DocumentId)
                {
                    this.RaiseChanged();
                }
            }

            public override void Connect()
            {
                _service.DiagnosticsUpdated += OnDiagnosticsUpdated;
            }

            public override void Disconnect()
            {
                _service.DiagnosticsUpdated -= OnDiagnosticsUpdated;
            }
        }
    }
}
