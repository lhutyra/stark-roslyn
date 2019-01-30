// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Composition;
using StarkPlatform.CodeAnalysis.AddFileBanner;
using StarkPlatform.CodeAnalysis.CodeRefactorings;

namespace StarkPlatform.CodeAnalysis.Stark.AddFileBanner
{
    [ExportCodeRefactoringProvider(LanguageNames.Stark,
        Name = PredefinedCodeRefactoringProviderNames.AddFileBanner), Shared]
    internal class CSharpAddFileBannerCodeRefactoringProvider : AbstractAddFileBannerCodeRefactoringProvider
    {
        protected override bool IsCommentStartCharacter(char ch)
            => ch == '/';
    }
}
