﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using StarkPlatform.CodeAnalysis.Classification;
using StarkPlatform.CodeAnalysis.Editor.Implementation.Classification;
using StarkPlatform.CodeAnalysis.Editor.Shared.Extensions;
using StarkPlatform.CodeAnalysis.Editor.Shared.Preview;
using StarkPlatform.CodeAnalysis.Editor.Shared.Utilities;
using StarkPlatform.CodeAnalysis.Host.Mef;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace StarkPlatform.CodeAnalysis.Editor.Implementation.Preview
{
    /// <summary>
    /// This tagger assumes content of the buffer never get changed. 
    /// and the buffer provides static classification information on the buffer content
    /// through <see cref="PredefinedPreviewTaggerKeys.StaticClassificationSpansKey" /> in the buffer property bag
    /// </summary>
    [Export(typeof(ITaggerProvider))]
    [TagType(typeof(IClassificationTag))]
    [ContentType(ContentTypeNames.StarkRoslynContentType)]
    [TextViewRole(TextViewRoles.PreviewRole)]
    internal class PreviewStaticClassificationTaggerProvider : ITaggerProvider
    {
        private readonly ClassificationTypeMap _typeMap;

        [ImportingConstructor]
        [Obsolete(MefConstruction.ImportingConstructorMessage, error: true)]
        public PreviewStaticClassificationTaggerProvider(ClassificationTypeMap typeMap)
        {
            _typeMap = typeMap;
        }

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer)
            where T : ITag
        {
            return new Tagger(_typeMap, buffer) as ITagger<T>;
        }

        private class Tagger : ITagger<IClassificationTag>
        {
            private readonly ClassificationTypeMap _typeMap;
            private readonly ITextBuffer _buffer;

            public Tagger(ClassificationTypeMap typeMap, ITextBuffer buffer)
            {
                _typeMap = typeMap;
                _buffer = buffer;
            }

            /// <summary>
            /// The tags never change for this tagger.
            /// </summary>
            event EventHandler<SnapshotSpanEventArgs> ITagger<IClassificationTag>.TagsChanged { add { } remove { } }

            public IEnumerable<ITagSpan<IClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
            {
                if (!_buffer.Properties.TryGetProperty(PredefinedPreviewTaggerKeys.StaticClassificationSpansKey, out ImmutableArray<ClassifiedSpan> classifiedSpans))
                {
                    yield break;
                }

                foreach (var span in spans)
                {
                    // we don't need to care about snapshot since everything is static and never changes in preview
                    var requestSpan = span.Span.ToTextSpan();

                    foreach (var classifiedSpan in classifiedSpans)
                    {
                        if (classifiedSpan.TextSpan.IntersectsWith(requestSpan))
                        {
                            yield return ClassificationUtilities.Convert(_typeMap, span.Snapshot, classifiedSpan);
                        }
                    }
                }
            }
        }
    }
}
