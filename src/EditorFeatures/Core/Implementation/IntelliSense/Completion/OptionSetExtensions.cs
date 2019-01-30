// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis.Completion;
using StarkPlatform.CodeAnalysis.Editor.Options;
using StarkPlatform.CodeAnalysis.Options;

namespace StarkPlatform.CodeAnalysis.Editor.Implementation.IntelliSense.Completion
{
    internal static class OptionSetExtensions
    {
        public static OptionSet WithDebuggerCompletionOptions(this OptionSet options)
        {
            return options
                .WithChangedOption(EditorCompletionOptions.UseSuggestionMode, options.GetOption(EditorCompletionOptions.UseSuggestionMode_Debugger))
                .WithChangedOption(CompletionControllerOptions.FilterOutOfScopeLocals, false)
                .WithChangedOption(CompletionControllerOptions.ShowXmlDocCommentCompletion, false);
        }
    }
}
