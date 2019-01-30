// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using Microsoft.VisualStudio;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.AddImports;
using StarkPlatform.CodeAnalysis.Stark;
using StarkPlatform.CodeAnalysis.Stark.Extensions;
using StarkPlatform.CodeAnalysis.Stark.Syntax;
using StarkPlatform.CodeAnalysis.Editor.Shared.Extensions;
using StarkPlatform.CodeAnalysis.Editor.Shared.Utilities;
using StarkPlatform.CodeAnalysis.Formatting;
using StarkPlatform.CodeAnalysis.Shared.Extensions;
using Microsoft.VisualStudio.Editor;
using StarkPlatform.VisualStudio.LanguageServices.CSharp.Snippets.SnippetFunctions;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.Snippets;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using MSXML;
using Roslyn.Utilities;
using VsTextSpan = Microsoft.VisualStudio.TextManager.Interop.TextSpan;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.Snippets
{
    internal sealed partial class SnippetExpansionClient : AbstractSnippetExpansionClient
    {
        public SnippetExpansionClient(IThreadingContext threadingContext, Guid languageServiceGuid, ITextView textView, ITextBuffer subjectBuffer, IVsEditorAdaptersFactoryService editorAdaptersFactoryService)
            : base(threadingContext, languageServiceGuid, textView, subjectBuffer, editorAdaptersFactoryService)
        {
        }

        /// <returns>The tracking span of the inserted "/**/" if there is an $end$ location, null
        /// otherwise.</returns>
        protected override ITrackingSpan InsertEmptyCommentAndGetEndPositionTrackingSpan()
        {
            VsTextSpan[] endSpanInSurfaceBuffer = new VsTextSpan[1];
            if (ExpansionSession.GetEndSpan(endSpanInSurfaceBuffer) != VSConstants.S_OK)
            {
                return null;
            }

            if (!TryGetSubjectBufferSpan(endSpanInSurfaceBuffer[0], out var subjectBufferEndSpan))
            {
                return null;
            }

            var endPosition = subjectBufferEndSpan.Start.Position;

            string commentString = "/**/";
            SubjectBuffer.Insert(endPosition, commentString);

            var commentSpan = new Span(endPosition, commentString.Length);
            return SubjectBuffer.CurrentSnapshot.CreateTrackingSpan(commentSpan, SpanTrackingMode.EdgeExclusive);
        }

        public override int GetExpansionFunction(IXMLDOMNode xmlFunctionNode, string bstrFieldName, out IVsExpansionFunction pFunc)
        {
            if (!TryGetSnippetFunctionInfo(xmlFunctionNode, out var snippetFunctionName, out var param))
            {
                pFunc = null;
                return VSConstants.E_INVALIDARG;
            }

            switch (snippetFunctionName)
            {
                case "SimpleTypeName":
                    pFunc = new SnippetFunctionSimpleTypeName(this, TextView, SubjectBuffer, bstrFieldName, param);
                    return VSConstants.S_OK;
                case "ClassName":
                    pFunc = new SnippetFunctionClassName(this, TextView, SubjectBuffer, bstrFieldName);
                    return VSConstants.S_OK;
                case "GenerateSwitchCases":
                    pFunc = new SnippetFunctionGenerateSwitchCases(this, TextView, SubjectBuffer, bstrFieldName, param);
                    return VSConstants.S_OK;
                default:
                    pFunc = null;
                    return VSConstants.E_INVALIDARG;
            }
        }

        internal override Document AddImports(
            Document document, int position, XElement snippetNode,
            bool placeSystemNamespaceFirst, CancellationToken cancellationToken)
        {
            var importsNode = snippetNode.Element(XName.Get("Imports", snippetNode.Name.NamespaceName));
            if (importsNode == null ||
                !importsNode.HasElements)
            {
                return document;
            }

            var root = document.GetSyntaxRootSynchronously(cancellationToken);
            var contextLocation = root.FindToken(position).Parent;

            var newUsingDirectives = GetUsingDirectivesToAdd(contextLocation, snippetNode, importsNode, cancellationToken);
            if (!newUsingDirectives.Any())
            {
                return document;
            }

            // In Venus/Razor, inserting imports statements into the subject buffer does not work.
            // Instead, we add the imports through the contained language host.
            if (TryAddImportsToContainedDocument(document, newUsingDirectives.Where(u => u.Alias == null).Select(u => u.Name.ToString())))
            {
                return document;
            }

            var addImportService = document.GetLanguageService<IAddImportsService>();
            var compilation = document.Project.GetCompilationAsync(cancellationToken).WaitAndGetResult(cancellationToken);
            var newRoot = addImportService.AddImports(compilation, root, contextLocation, newUsingDirectives, placeSystemNamespaceFirst);

            var newDocument = document.WithSyntaxRoot(newRoot);

            var formattedDocument = Formatter.FormatAsync(newDocument, Formatter.Annotation, cancellationToken: cancellationToken).WaitAndGetResult(cancellationToken);
            document.Project.Solution.Workspace.ApplyDocumentChanges(formattedDocument, cancellationToken);

            return formattedDocument;
        }

        private static IList<ImportDirectiveSyntax> GetUsingDirectivesToAdd(
            SyntaxNode contextLocation, XElement snippetNode, XElement importsNode, CancellationToken cancellationToken)
        {
            var namespaceXmlName = XName.Get("Namespace", snippetNode.Name.NamespaceName);
            var existingUsings = contextLocation.GetEnclosingUsingDirectives();
            var newUsings = new List<ImportDirectiveSyntax>();

            foreach (var import in importsNode.Elements(XName.Get("Import", snippetNode.Name.NamespaceName)))
            {
                var namespaceElement = import.Element(namespaceXmlName);
                if (namespaceElement == null)
                {
                    continue;
                }

                var namespaceToImport = namespaceElement.Value.Trim();
                if (string.IsNullOrEmpty(namespaceToImport))
                {
                    continue;
                }

                var candidateUsing = SyntaxFactory.ParseCompilationUnit("using " + namespaceToImport + ";").DescendantNodes().OfType<ImportDirectiveSyntax>().FirstOrDefault();
                if (candidateUsing == null)
                {
                    continue;
                }

                if (!existingUsings.Any(u => u.IsEquivalentTo(candidateUsing, topLevel: false)))
                {
                    newUsings.Add(candidateUsing.WithAdditionalAnnotations(Formatter.Annotation).WithAppendedTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed));
                }
            }

            return newUsings;
        }

        private static string GetAliasName(ImportDirectiveSyntax usingDirective)
        {
            return (usingDirective.Alias == null || usingDirective.Alias.Name == null) ? null : usingDirective.Alias.Name.ToString();
        }
    }
}
