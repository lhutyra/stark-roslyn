// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Editor;
using Microsoft.VisualStudio.ComponentModelHost;
using StarkPlatform.VisualStudio.LanguageServices.Implementation;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.LanguageService
{
    [ExcludeFromCodeCoverage]
    [Guid(Guids.StarkEditorFactoryIdString)]
    internal class CSharpEditorFactory : AbstractEditorFactory
    {
        public CSharpEditorFactory(IComponentModel componentModel)
            : base(componentModel)
        {
        }

        protected override string ContentTypeName => ContentTypeNames.StarkContentType;
        protected override string LanguageName => LanguageNames.Stark;
    }
}
