// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.AddImport;
using Microsoft.VisualStudio.OLE.Interop;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.ProjectSystem
{
    internal partial class VisualStudioWorkspaceImpl
    {
        private abstract class AbstractAddRemoveUndoUnit : IOleUndoUnit
        {
            protected readonly ProjectId FromProjectId;
            protected readonly VisualStudioWorkspaceImpl Workspace;

            protected AbstractAddRemoveUndoUnit(
                VisualStudioWorkspaceImpl workspace,
                ProjectId fromProjectId)
            {
                Workspace = workspace;
                FromProjectId = fromProjectId;
            }

            public abstract void Do(IOleUndoManager pUndoManager);
            public abstract void GetDescription(out string pBstr);

            public void GetUnitType(out Guid pClsid, out int plID)
            {
                throw new NotImplementedException();
            }

            public void OnNextAdd()
            {
            }
        }
    }
}
