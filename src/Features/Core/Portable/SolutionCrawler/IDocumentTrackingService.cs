﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using StarkPlatform.CodeAnalysis.Host;

namespace StarkPlatform.CodeAnalysis
{
    internal interface IDocumentTrackingService : IWorkspaceService
    {
        /// <summary>
        /// Get the <see cref="DocumentId"/> of the active document. May be null if there is no active document
        /// or the active document is not in the workspace.
        /// </summary>
        DocumentId TryGetActiveDocument();

        /// <summary>
        /// Get a read only collection of the <see cref="DocumentId"/>s of all the visible documents in the workspace.
        /// </summary>
        ImmutableArray<DocumentId> GetVisibleDocuments();

        event EventHandler<DocumentId> ActiveDocumentChanged;

        /// <summary>
        /// Raised when a text buffer that's not part of a workspace is changed.
        /// </summary>
        event EventHandler<EventArgs> NonRoslynBufferTextChanged;
    }
}
