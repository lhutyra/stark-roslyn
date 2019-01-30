// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using StarkPlatform.CodeAnalysis.Editor.Implementation.EndConstructGeneration;
using StarkPlatform.CodeAnalysis.Host.Mef;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace StarkPlatform.CodeAnalysis.Editor.CSharp.EndConstructGeneration
{
    [ExportLanguageService(typeof(IEndConstructGenerationService), LanguageNames.Stark), Shared]
    [ExcludeFromCodeCoverage]
    internal class CSharpEndConstructGenerationService : IEndConstructGenerationService
    {
        public bool TryDo(
            ITextView textView,
            ITextBuffer subjectBuffer,
            char typedChar,
            CancellationToken cancellationToken)
        {
            return false;
        }
    }
}
