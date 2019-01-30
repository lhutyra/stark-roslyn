// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Editor;
using StarkPlatform.CodeAnalysis.Formatting;
using StarkPlatform.CodeAnalysis.Snippets;
using StarkPlatform.VisualStudio.LanguageServices.CSharp.ProjectSystemShim;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.DebuggerIntelliSense;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.LanguageService;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.ProjectSystem;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.LanguageService
{
    [ExcludeFromCodeCoverage]
    [Guid(Guids.StarkLanguageServiceIdString)]
    internal partial class StarkLanguageService : AbstractLanguageService<StarkPackage, StarkLanguageService>
    {
        internal StarkLanguageService(StarkPackage package)
            : base(package)
        {
        }

        protected override Guid DebuggerLanguageId
        {
            get
            {
                return Guids.StarkDebuggerLanguageId;
            }
        }

        protected override string ContentTypeName
        {
            get
            {
                return ContentTypeNames.StarkContentType;
            }
        }

        public override Guid LanguageServiceId
        {
            get
            {
                return Guids.StarkLanguageServiceId;
            }
        }

        protected override string LanguageName
        {
            get
            {
                return CSharpVSResources.CSharp;
            }
        }

        protected override string RoslynLanguageName
        {
            get
            {
                return LanguageNames.Stark;
            }
        }

        protected override AbstractDebuggerIntelliSenseContext CreateContext(
            IWpfTextView view,
            IVsTextView vsTextView,
            IVsTextLines debuggerBuffer,
            ITextBuffer subjectBuffer,
            Microsoft.VisualStudio.TextManager.Interop.TextSpan[] currentStatementSpan)
        {
            return new CSharpDebuggerIntelliSenseContext(view,
                vsTextView,
                debuggerBuffer,
                subjectBuffer,
                currentStatementSpan,
                this.Package.ComponentModel,
                this.SystemServiceProvider);
        }
    }
}
