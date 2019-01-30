// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Composition;
using StarkPlatform.CodeAnalysis.Editor;
using StarkPlatform.CodeAnalysis.Host;
using StarkPlatform.CodeAnalysis.Host.Mef;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;

namespace StarkPlatform.CodeAnalysis.Text.Implementation.TextBufferFactoryService
{
    [ExportWorkspaceServiceFactory(typeof(ITextBufferCloneService), ServiceLayer.Editor), Shared]
    internal class TextBufferCloneServiceFactory : IWorkspaceServiceFactory
    {
        private readonly ITextBufferCloneService _singleton;

        [ImportingConstructor]
        public TextBufferCloneServiceFactory(
            ITextBufferFactoryService textBufferFactoryService,
            IContentTypeRegistryService contentTypeRegistryService)
        {
            _singleton = new TextBufferCloneService((ITextBufferFactoryService3)textBufferFactoryService, contentTypeRegistryService);
        }

        public IWorkspaceService CreateService(HostWorkspaceServices workspaceServices)
        {
            return _singleton;
        }

        [Export(typeof(ITextBufferCloneService)), Shared]
        private class TextBufferCloneService : ITextBufferCloneService
        {
            private readonly ITextBufferFactoryService3 _textBufferFactoryService;
            private readonly IContentType _roslynContentType;
            private readonly IContentType _unknownContentType;

            [ImportingConstructor]
            public TextBufferCloneService(ITextBufferFactoryService3 textBufferFactoryService, IContentTypeRegistryService contentTypeRegistryService)
            {
                _textBufferFactoryService = textBufferFactoryService;

                _roslynContentType = contentTypeRegistryService.GetContentType(ContentTypeNames.StarkRoslynContentType);
                _unknownContentType = contentTypeRegistryService.UnknownContentType;
            }

            public ITextBuffer CloneWithUnknownContentType(SnapshotSpan span)
                => _textBufferFactoryService.CreateTextBuffer(span, _unknownContentType);

            public ITextBuffer CloneWithUnknownContentType(ITextImage textImage)
                => Clone(textImage, _unknownContentType);

            public ITextBuffer CloneWithRoslynContentType(SourceText sourceText)
                => Clone(sourceText, _roslynContentType);

            public ITextBuffer Clone(SourceText sourceText, IContentType contentType)
            {
                // see whether we can do it cheaply
                var textImage = sourceText.TryFindCorrespondingEditorTextImage();
                if (textImage != null)
                {
                    return Clone(textImage, contentType);
                }

                // we can't, so do it more expensive way
                return _textBufferFactoryService.CreateTextBuffer(sourceText.ToString(), contentType);
            }

            private ITextBuffer Clone(ITextImage textImage, IContentType contentType)
                => _textBufferFactoryService.CreateTextBuffer(textImage, contentType);
        }
    }
}
