// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis.Editor.Implementation.BraceMatching;

namespace StarkPlatform.CodeAnalysis.Editor.CSharp.BraceMatching
{
    [ExportBraceMatcher(LanguageNames.Stark)]
    internal class CSharpEmbeddedLanguageBraceMatcher : AbstractEmbeddedLanguageBraceMatcher
    {
    }
}
