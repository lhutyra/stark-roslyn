// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using StarkPlatform.CodeAnalysis.Diagnostics;
using StarkPlatform.CodeAnalysis.Host;

namespace StarkPlatform.CodeAnalysis.Editor.Host
{
    internal interface IPreviewPaneService : IWorkspaceService
    {
        object GetPreviewPane(DiagnosticData diagnostic, string language, string projectType, IReadOnlyList<object> previewContent);
    }
}
