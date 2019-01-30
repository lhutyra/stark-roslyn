// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis.Editor.Implementation.IntelliSense.Completion;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace StarkPlatform.CodeAnalysis.Editor
{
    internal interface IAsyncCompletionService
    {
        bool TryGetController(ITextView textView, ITextBuffer subjectBuffer, out Controller controller);
    }
}
