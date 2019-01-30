// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis.Stark;

namespace StarkPlatform.CodeAnalysis.Editor.CSharp.BraceMatching
{
    [ExportBraceMatcher(LanguageNames.Stark)]
    internal class OpenCloseBracketBraceMatcher : AbstractCSharpBraceMatcher
    {
        public OpenCloseBracketBraceMatcher()
            : base(SyntaxKind.OpenBracketToken, SyntaxKind.CloseBracketToken)
        {
        }
    }
}
