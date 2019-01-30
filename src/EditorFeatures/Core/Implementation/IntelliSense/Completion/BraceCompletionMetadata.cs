﻿using System.Collections.Generic;
using Roslyn.Utilities;

namespace StarkPlatform.CodeAnalysis.Editor.Implementation.IntelliSense.Completion
{
    internal class BraceCompletionMetadata
    {
        public IEnumerable<char> OpeningBraces { get; }
        public IEnumerable<char> ClosingBraces { get; }
        public IEnumerable<string> ContentTypes { get; }

        public BraceCompletionMetadata(IReadOnlyDictionary<string, object> data)
        {
            OpeningBraces = data.GetEnumerableMetadata<char>(nameof(OpeningBraces));
            ClosingBraces = data.GetEnumerableMetadata<char>(nameof(ClosingBraces));
            ContentTypes = data.GetEnumerableMetadata<string>(nameof(ContentTypes));
        }
    }
}
