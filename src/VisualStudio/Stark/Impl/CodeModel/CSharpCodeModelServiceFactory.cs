// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Composition;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Editor;
using StarkPlatform.CodeAnalysis.Host;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.CodeModel;
using Microsoft.VisualStudio.Text.Editor;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.CodeModel
{
    [ExportLanguageServiceFactory(typeof(ICodeModelService), LanguageNames.Stark), Shared]
    internal partial class CSharpCodeModelServiceFactory : ILanguageServiceFactory
    {
        private readonly IEditorOptionsFactoryService _editorOptionsFactoryService;
        private readonly IEnumerable<IRefactorNotifyService> _refactorNotifyServices;

        [ImportingConstructor]
        private CSharpCodeModelServiceFactory(
            IEditorOptionsFactoryService editorOptionsFactoryService,
            [ImportMany] IEnumerable<IRefactorNotifyService> refactorNotifyServices)
        {
            _editorOptionsFactoryService = editorOptionsFactoryService;
            _refactorNotifyServices = refactorNotifyServices;
        }

        public ILanguageService CreateLanguageService(HostLanguageServices provider)
        {
            return new CSharpCodeModelService(provider, _editorOptionsFactoryService, _refactorNotifyServices);
        }
    }
}
