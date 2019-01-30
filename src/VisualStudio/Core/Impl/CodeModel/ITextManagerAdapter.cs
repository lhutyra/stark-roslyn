// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis.Editor.Shared.Utilities;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.CodeModel
{
    /// <summary>
    /// A shim interface over the TextManager APIs needed to isolate unit tests.
    /// </summary>
    internal interface ITextManagerAdapter
    {
        EnvDTE.TextPoint CreateTextPoint(FileCodeModel fileCodeModel, VirtualTreePoint point);
    }
}
