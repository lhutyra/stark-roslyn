// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Editor;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.ProjectSystem;
using Microsoft.VisualStudio.Shell.TableManager;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.TableDataSource
{
    [Export(typeof(MiscellaneousTodoListTable))]
    internal class MiscellaneousTodoListTable : VisualStudioBaseTodoListTable
    {
        internal const string IdentifierString = nameof(MiscellaneousTodoListTable);

        [ImportingConstructor]
        public MiscellaneousTodoListTable(MiscellaneousFilesWorkspace workspace, ITodoListProvider todoListProvider, ITableManagerProvider provider) :
            base(workspace, todoListProvider, IdentifierString, provider)
        {
            ConnectWorkspaceEvents();
        }

        // only for test
        public MiscellaneousTodoListTable(StarkPlatform.CodeAnalysis.Workspace workspace, ITodoListProvider todoListProvider, ITableManagerProvider provider) :
            base(workspace, todoListProvider, IdentifierString, provider)
        {
        }
    }
}
