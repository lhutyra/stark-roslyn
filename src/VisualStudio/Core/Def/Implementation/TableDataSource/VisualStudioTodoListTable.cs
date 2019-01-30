﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Editor;
using Microsoft.VisualStudio.Shell.TableManager;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.TableDataSource
{
    using Workspace = StarkPlatform.CodeAnalysis.Workspace;

    [Export(typeof(VisualStudioTodoListTable))]
    internal class VisualStudioTodoListTable : VisualStudioBaseTodoListTable
    {
        internal const string IdentifierString = nameof(VisualStudioTodoListTable);

        [ImportingConstructor]
        public VisualStudioTodoListTable(VisualStudioWorkspace workspace, ITodoListProvider todoListProvider, ITableManagerProvider provider) :
            base(workspace, todoListProvider, IdentifierString, provider)
        {
            ConnectWorkspaceEvents();
        }

        // only for test
        public VisualStudioTodoListTable(Workspace workspace, ITodoListProvider todoListProvider, ITableManagerProvider provider) :
            base(workspace, todoListProvider, IdentifierString, provider)
        {
        }
    }
}
