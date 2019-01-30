// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Composition;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Editor.Shared.Utilities;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.CodeAnalysis.Shared.TestHooks;
using StarkPlatform.CodeAnalysis.Snippets;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.Snippets;
using Microsoft.VisualStudio.Shell;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.Snippets
{
    // HACK: The Export attribute (as ISnippetInfoService) is used by EditorTestApp to create this
    // SnippetInfoService on the UI thread.
    [Export(typeof(ISnippetInfoService))]
    [ExportLanguageService(typeof(ISnippetInfoService), LanguageNames.Stark), Shared]
    internal class CSharpSnippetInfoService : AbstractSnippetInfoService
    {
        // #region and #endregion when appears in the completion list as snippets
        // we should format the snippet on commit. 
        private ISet<string> _formatTriggeringSnippets = new HashSet<string>(new string[] { "#region", "#endregion" });

        [ImportingConstructor]
        public CSharpSnippetInfoService(
            IThreadingContext threadingContext,
            SVsServiceProvider serviceProvider,
            IAsynchronousOperationListenerProvider listenerProvider)
            : base(threadingContext, serviceProvider, Guids.StarkLanguageServiceId, listenerProvider)
        {
        }

        public override bool ShouldFormatSnippet(SnippetInfo snippetInfo)
        {
            return _formatTriggeringSnippets.Contains(snippetInfo.Shortcut);
        }
    }
}
