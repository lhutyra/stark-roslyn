﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using StarkPlatform.CodeAnalysis.Completion;
using StarkPlatform.CodeAnalysis.Completion.Providers;
using StarkPlatform.CodeAnalysis.Editor.Shared.Extensions;
using StarkPlatform.CodeAnalysis.Editor.Shared.Utilities;
using StarkPlatform.CodeAnalysis.LanguageServices;
using StarkPlatform.CodeAnalysis.PooledObjects;
using StarkPlatform.CodeAnalysis.Shared.Extensions;
using StarkPlatform.CodeAnalysis.Text;
using StarkPlatform.CodeAnalysis.Text.Shared.Extensions;
using Microsoft.VisualStudio.Core.Imaging;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Editor;
using AsyncCompletionData = Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion.Data;
using RoslynCompletionItem = StarkPlatform.CodeAnalysis.Completion.CompletionItem;
using VSCompletionItem = Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion.Data.CompletionItem;

namespace StarkPlatform.CodeAnalysis.Editor.Implementation.IntelliSense.AsyncCompletion
{
    internal class CompletionSource : ForegroundThreadAffinitizedObject, IAsyncCompletionSource
    {
        internal const string RoslynItem = nameof(RoslynItem);
        internal const string TriggerSnapshot = nameof(TriggerSnapshot);
        internal const string InsertionText = nameof(InsertionText);
        internal const string HasSuggestionItemOptions = nameof(HasSuggestionItemOptions);
        internal const string Description = nameof(Description);
        internal const string InitialTriggerKind = nameof(InitialTriggerKind);
        internal const string PotentialCommitCharacters = nameof(PotentialCommitCharacters);
        internal const string ExcludedCommitCharacters = nameof(ExcludedCommitCharacters);
        internal const string NonBlockingCompletion = nameof(NonBlockingCompletion);

        private static readonly ImmutableArray<ImageElement> s_WarningImageAttributeImagesArray =
            ImmutableArray.Create(new ImageElement(Glyph.CompletionWarning.GetImageId(), EditorFeaturesResources.Warning_image_element));

        private static readonly EditorOptionKey<bool> NonBlockingCompletionEditorOption = new EditorOptionKey<bool>(NonBlockingCompletion);

        private readonly ITextView _textView;

        internal CompletionSource(ITextView textView, IThreadingContext threadingContext) : base(threadingContext)
        {
            _textView = textView;
        }

        public AsyncCompletionData.CompletionStartData InitializeCompletion(
            AsyncCompletionData.CompletionTrigger trigger,
            SnapshotPoint triggerLocation,
            CancellationToken cancellationToken)
        {
            // We take sourceText from document to get a snapshot span.
            // We would like to be sure that nobody changes buffers at the same time.
            AssertIsForeground();

            var document = triggerLocation.Snapshot.GetOpenDocumentInCurrentContextWithChanges();
            if (document == null)
            {
                return AsyncCompletionData.CompletionStartData.DoesNotParticipateInCompletion;
            }

            var service = document.GetLanguageService<CompletionService>();
            if (service == null)
            {
                return AsyncCompletionData.CompletionStartData.DoesNotParticipateInCompletion;
            }

            if (!document.Project.Solution.Workspace.Options.GetOption(CompletionOptions.BlockForCompletionItems, service.Language))
            {
                _textView.Options.GlobalOptions.SetOptionValue(NonBlockingCompletionEditorOption, true);
            }

            // In case of calls with multiple completion services for the same view (e.g. TypeScript and C#), those completion services must not be called simultaneously for the same session.
            // Therefore, in each completion session we use a list of commit character for a specific completion service and a specific content type.
            _textView.Properties[PotentialCommitCharacters] = service.GetRules().DefaultCommitCharacters;

            var sourceText = document.GetTextSynchronously(cancellationToken);

            return ShouldTriggerCompletion(trigger, triggerLocation, sourceText, document, service)
                ? new AsyncCompletionData.CompletionStartData(
                    participation: AsyncCompletionData.CompletionParticipation.ProvidesItems,
                    applicableToSpan: new SnapshotSpan(
                        triggerLocation.Snapshot,
                        service.GetDefaultCompletionListSpan(sourceText, triggerLocation.Position).ToSpan()))
                : AsyncCompletionData.CompletionStartData.DoesNotParticipateInCompletion;
        }

        private bool ShouldTriggerCompletion(
            AsyncCompletionData.CompletionTrigger trigger,
            SnapshotPoint triggerLocation,
            SourceText sourceText,
            Document document,
            CompletionService completionService)
        {
            // The trigger reason guarantees that user wants a completion.
            if (trigger.Reason == AsyncCompletionData.CompletionTriggerReason.Invoke || 
                trigger.Reason == AsyncCompletionData.CompletionTriggerReason.InvokeAndCommitIfUnique)
            {
                return true;
            }

            //The user may be trying to invoke snippets through question-tab.
            // We may provide a completion after that.
            // Otherwise, tab should not be a completion trigger.
            if (trigger.Reason == AsyncCompletionData.CompletionTriggerReason.Insertion && trigger.Character == '\t')
            {
                return TryInvokeSnippetCompletion(completionService, document, sourceText, triggerLocation.Position);
            }

            var roslynTrigger = Helpers.GetRoslynTrigger(trigger, triggerLocation);

            // The completion service decides that user may want a completion.
            if (completionService.ShouldTriggerCompletion(sourceText, triggerLocation.Position, roslynTrigger))
            {
                return true;
            }

            return false;
        }

        private static bool TryInvokeSnippetCompletion(
            CompletionService completionService, Document document, SourceText text, int caretPoint)
        {
            var rules = completionService.GetRules();
            // Do not invoke snippet if the corresponding rule is not set in options.
            if (rules.SnippetsRule != SnippetsRule.IncludeAfterTypingIdentifierQuestionTab)
            {
                return false;
            }

            var syntaxFactsOpt = document.GetLanguageService<ISyntaxFactsService>();
            // Snippets are included if the user types: <quesiton><tab>
            // If at least one condition for snippets do not hold, bail out.
            if (syntaxFactsOpt == null ||
                caretPoint < 3 ||
                text[caretPoint - 2] != '?' ||
                !QuestionMarkIsPrecededByIdentifierAndWhitespace(text, caretPoint - 2, syntaxFactsOpt))
            {
                return false;
            }

            // Because <question><tab> is actually a command to bring up snippets,
            // we delete the last <question> that was typed.
            var textChange = new TextChange(TextSpan.FromBounds(caretPoint - 2, caretPoint), string.Empty);
            document.Project.Solution.Workspace.ApplyTextChanges(document.Id, textChange, CancellationToken.None);

            return true;
        }

        public async Task<AsyncCompletionData.CompletionContext> GetCompletionContextAsync(
            IAsyncCompletionSession session,
            AsyncCompletionData.CompletionTrigger trigger,
            SnapshotPoint triggerLocation,
            SnapshotSpan applicableToSpan,
            CancellationToken cancellationToken)
        {
            var document = triggerLocation.Snapshot.GetOpenDocumentInCurrentContextWithChanges();
            if (document == null)
            {
                return new AsyncCompletionData.CompletionContext(ImmutableArray<VSCompletionItem>.Empty);
            }

            var completionService = document.GetLanguageService<CompletionService>();

            var roslynTrigger = Helpers.GetRoslynTrigger(trigger, triggerLocation);

            var completionList = await completionService.GetCompletionsAsync(
                document,
                triggerLocation,
                roslynTrigger).ConfigureAwait(false);

            if (completionList == null)
            {
                return new AsyncCompletionData.CompletionContext(ImmutableArray<VSCompletionItem>.Empty);
            }

            var filterCache = new Dictionary<string, AsyncCompletionData.CompletionFilter>();

            var itemsBuilder = new ArrayBuilder<VSCompletionItem>(completionList.Items.Length);
            foreach (var roslynItem in completionList.Items)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var item = Convert(document, roslynItem, completionService, filterCache);
                itemsBuilder.Add(item);
            }

            var items = itemsBuilder.ToImmutableAndFree();

            var suggestionItemOptions = completionList.SuggestionModeItem != null
                    ? new AsyncCompletionData.SuggestionItemOptions(
                        completionList.SuggestionModeItem.DisplayText,
                        completionList.SuggestionModeItem.Properties.TryGetValue(Description, out var description)
                            ? description
                            : string.Empty)
                    : null;

            // Have to store the snapshot to reuse it in some projections related scenarios
            // where data and session in further calls are able to provide other snapshots.
            session.Properties.AddProperty(TriggerSnapshot, triggerLocation.Snapshot);

            // This is a code supporting original completion scenarios: 
            // Controller.Session_ComputeModel: if completionList.SuggestionModeItem != null, then suggestionMode = true
            // If there are suggestionItemOptions, then later HandleNormalFiltering should set selection to SoftSelection.
            session.Properties.AddProperty(HasSuggestionItemOptions, suggestionItemOptions != null);

            session.Properties.AddProperty(InitialTriggerKind, roslynTrigger.Kind);
            var excludedCommitCharacters = GetExcludedCommitCharacters(completionList.Items);
            if (excludedCommitCharacters.Length > 0)
            {
                session.Properties.AddProperty(ExcludedCommitCharacters, excludedCommitCharacters);
            }

            return new AsyncCompletionData.CompletionContext(
                items,
                suggestionItemOptions,
                suggestionItemOptions == null
                    ? AsyncCompletionData.InitialSelectionHint.RegularSelection
                    : AsyncCompletionData.InitialSelectionHint.SoftSelection);
        }

        public async Task<object> GetDescriptionAsync(IAsyncCompletionSession session, VSCompletionItem item, CancellationToken cancellationToken)
        {
            if (!item.Properties.TryGetProperty(RoslynItem, out RoslynCompletionItem roslynItem) ||
                !session.Properties.TryGetProperty(TriggerSnapshot, out ITextSnapshot triggerSnapshot))
            {
                return null;
            }

            var document = triggerSnapshot.GetOpenDocumentInCurrentContextWithChanges();
            if (document == null)
            {
                return null;
            }

            var service = document.GetLanguageService<CompletionService>();

            if (service == null)
            {
                return null;
            }

            var description = await service.GetDescriptionAsync(document, roslynItem, cancellationToken).ConfigureAwait(false);

            return IntelliSense.Helpers.BuildClassifiedTextElement(description.TaggedParts);
        }

        private VSCompletionItem Convert(
            Document document,
            RoslynCompletionItem roslynItem,
            CompletionService completionService,
            Dictionary<string, AsyncCompletionData.CompletionFilter> filterCache)
        {
            var imageId = roslynItem.Tags.GetFirstGlyph().GetImageId();
            var filters = GetFilters(roslynItem, filterCache);

            // roslynItem generated by providers can contain an insertionText in a property bag.
            // We will not use it but other providers may need it.
            // We actually will calculate the insertion text once again when called TryCommit.
            if (!roslynItem.Properties.TryGetValue(InsertionText, out var insertionText))
            {
                insertionText = roslynItem.DisplayText;
            }

            var supportedPlatforms = SymbolCompletionItem.GetSupportedPlatforms(roslynItem, document.Project.Solution.Workspace);
            var attributeImages = supportedPlatforms != null ? s_WarningImageAttributeImagesArray : ImmutableArray<ImageElement>.Empty;

            var item = new VSCompletionItem(
                displayText: roslynItem.GetEntireDisplayText(),
                source: this,
                icon: new ImageElement(new ImageId(imageId.Guid, imageId.Id), roslynItem.DisplayText),
                filters: filters,
                suffix: string.Empty, // Do not use the suffix unless want it to be right-aligned in the selection popup
                insertText: insertionText,
                sortText: roslynItem.SortText,
                filterText: roslynItem.FilterText,
                attributeIcons: attributeImages);

            item.Properties.AddProperty(RoslynItem, roslynItem);
            return item;
        }

        private ImmutableArray<char> GetExcludedCommitCharacters(ImmutableArray<RoslynCompletionItem> roslynItems)
        {
            var hashSet = new HashSet<char>();
            foreach (var roslynItem in roslynItems)
            {
                foreach (var rule in roslynItem.Rules?.FilterCharacterRules)
                {
                    if (rule.Kind == CharacterSetModificationKind.Add)
                    {
                        foreach (var c in rule.Characters)
                        {
                            hashSet.Add(c);
                        }
                    }
                }
            }

            return hashSet.ToImmutableArray();
        }

        private ImmutableArray<AsyncCompletionData.CompletionFilter> GetFilters(RoslynCompletionItem item, Dictionary<string, AsyncCompletionData.CompletionFilter> filterCache)
        {
            var listBuilder = new ArrayBuilder<AsyncCompletionData.CompletionFilter>();
            foreach (var filter in CompletionItemFilter.AllFilters)
            {
                if (filter.Matches(item))
                {
                    if (!filterCache.TryGetValue(filter.DisplayText, out var itemFilter))
                    {
                        var imageId = filter.Tags.GetFirstGlyph().GetImageId();
                        itemFilter = new AsyncCompletionData.CompletionFilter(
                            filter.DisplayText,
                            filter.AccessKey.ToString(),
                            new ImageElement(new ImageId(imageId.Guid, imageId.Id), EditorFeaturesResources.Filter_image_element));
                        filterCache[filter.DisplayText] = itemFilter;
                    }

                    listBuilder.Add(itemFilter);
                }
            }

            return listBuilder.ToImmutableAndFree();
        }

        internal static bool QuestionMarkIsPrecededByIdentifierAndWhitespace(
            SourceText text, int questionPosition, ISyntaxFactsService syntaxFacts)
        {
            var startOfLine = text.Lines.GetLineFromPosition(questionPosition).Start;

            // First, skip all the whitespace.
            var current = startOfLine;
            while (current < questionPosition && char.IsWhiteSpace(text[current]))
            {
                current++;
            }

            if (current < questionPosition && syntaxFacts.IsIdentifierStartCharacter(text[current]))
            {
                current++;
            }
            else
            {
                return false;
            }

            while (current < questionPosition && syntaxFacts.IsIdentifierPartCharacter(text[current]))
            {
                current++;
            }

            return current == questionPosition;
        }
    }
}
