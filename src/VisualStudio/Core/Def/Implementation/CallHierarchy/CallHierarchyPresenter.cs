// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Editor.Host;
using StarkPlatform.CodeAnalysis.Editor.Implementation.CallHierarchy;
using Microsoft.VisualStudio.CallHierarchy.Package.Definitions;
using Microsoft.VisualStudio.Language.CallHierarchy;
using Microsoft.VisualStudio.Shell;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.CallHierarchy
{
    [Export(typeof(ICallHierarchyPresenter))]
    internal class CallHierarchyPresenter : ICallHierarchyPresenter
    {
        private readonly IServiceProvider _serviceProvider;

        [ImportingConstructor]
        public CallHierarchyPresenter(SVsServiceProvider serviceProvider)
        {
            _serviceProvider = (IServiceProvider)serviceProvider;
        }

        public void PresentRoot(CallHierarchyItem root)
        {
            var callHierarchy = _serviceProvider.GetService(typeof(SCallHierarchy)) as ICallHierarchy;
            callHierarchy.ShowToolWindow();
            callHierarchy.AddRootItem((ICallHierarchyMemberItem)root);
        }
    }
}
