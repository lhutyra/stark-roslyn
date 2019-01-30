// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis.Stark;
using StarkPlatform.CodeAnalysis.Editor.Implementation.AutomaticCompletion.Sessions;
using StarkPlatform.CodeAnalysis.LanguageServices;

namespace StarkPlatform.CodeAnalysis.Editor.CSharp.AutomaticCompletion.Sessions
{
    internal class BracketCompletionSession : AbstractTokenBraceCompletionSession
    {
        public BracketCompletionSession(ISyntaxFactsService syntaxFactsService)
            : base(syntaxFactsService, (int)SyntaxKind.OpenBracketToken, (int)SyntaxKind.CloseBracketToken)
        {
        }
    }
}
