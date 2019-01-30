// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using StarkPlatform.CodeAnalysis.Host;
using StarkPlatform.CodeAnalysis.Text;

namespace StarkPlatform.CodeAnalysis.Editor.Implementation.CommentSelection
{
    [Obsolete("Use StarkPlatform.CodeAnalysis.CommentSelection.ICommentSelectionService instead")]
    internal interface ICommentUncommentService : ILanguageService
    {
        string SingleLineCommentString { get; }
        bool SupportsBlockComment { get; }
        string BlockCommentStartString { get; }
        string BlockCommentEndString { get; }

        Document Format(Document document, IEnumerable<TextSpan> changes, CancellationToken cancellationToken);
    }
}
