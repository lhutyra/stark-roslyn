// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Composition;
using StarkPlatform.CodeAnalysis.CodeRefactorings;
using StarkPlatform.CodeAnalysis.Host;
using StarkPlatform.CodeAnalysis.Host.Mef;

namespace StarkPlatform.CodeAnalysis.Editor.Implementation.CodeRefactorings
{
    [ExportWorkspaceServiceFactory(typeof(ICodeRefactoringHelpersService), ServiceLayer.Editor), Shared]
    internal class EditorLayerCodeActionHelpersService : IWorkspaceServiceFactory
    {
        private readonly IInlineRenameService _renameService;

        [ImportingConstructor]
        public EditorLayerCodeActionHelpersService(IInlineRenameService renameService)
        {
            _renameService = renameService;
        }

        public IWorkspaceService CreateService(HostWorkspaceServices workspaceServices)
        {
            return new CodeActionHelpersService(this);
        }

        private class CodeActionHelpersService : ICodeRefactoringHelpersService
        {
            private readonly EditorLayerCodeActionHelpersService _service;

            public CodeActionHelpersService(EditorLayerCodeActionHelpersService service)
            {
                _service = service;
            }

            public bool ActiveInlineRenameSession
            {
                get
                {
                    return _service._renameService.ActiveSession != null;
                }
            }
        }
    }
}
