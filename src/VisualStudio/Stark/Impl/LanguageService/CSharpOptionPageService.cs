// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Composition;
using Microsoft.VisualStudio;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Editor;
using StarkPlatform.CodeAnalysis.Editor.Shared.Utilities;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.VisualStudio.LanguageServices.CSharp.Options.Formatting;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.LanguageService
{
    [ExportLanguageService(typeof(IOptionPageService), LanguageNames.Stark), Shared]
    internal class CSharpOptionPageService : ForegroundThreadAffinitizedObject, IOptionPageService
    {
        private readonly StarkPackage _package;

        [ImportingConstructor]
        [Obsolete(MefConstruction.ImportingConstructorMessage, error: true)]
        public CSharpOptionPageService(IThreadingContext threadingContext, SVsServiceProvider serviceProvider)
            : base(threadingContext)
        {
            var shell = (IVsShell)serviceProvider.GetService(typeof(SVsShell));
            ErrorHandler.ThrowOnFailure(shell.LoadPackage(Guids.StarkPackageId, out var package));
            _package = (StarkPackage)package;
        }

        public void ShowFormattingOptionPage()
        {
            AssertIsForeground();

            _package.ShowOptionPage(typeof(FormattingOptionPage));
        }
    }
}
