// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Windows;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Completion;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.Options;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.Options
{
    internal partial class IntelliSenseOptionPageControl : AbstractOptionPageControl
    {
        public IntelliSenseOptionPageControl(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            InitializeComponent();

            BindToOption(Show_completion_item_filters, CompletionOptions.ShowCompletionItemFilters, LanguageNames.Stark);
            BindToOption(Highlight_matching_portions_of_completion_list_items, CompletionOptions.HighlightMatchingPortionsOfCompletionListItems, LanguageNames.Stark);

            BindToOption(Show_completion_list_after_a_character_is_typed, CompletionOptions.TriggerOnTypingLetters, LanguageNames.Stark);
            Show_completion_list_after_a_character_is_deleted.IsChecked = this.OptionService.GetOption(
                CompletionOptions.TriggerOnDeletion, LanguageNames.Stark) == true;
            Show_completion_list_after_a_character_is_deleted.IsEnabled = Show_completion_list_after_a_character_is_typed.IsChecked == true;

            BindToOption(Never_include_snippets, CompletionOptions.SnippetsBehavior, SnippetsRule.NeverInclude, LanguageNames.Stark);
            BindToOption(Always_include_snippets, CompletionOptions.SnippetsBehavior, SnippetsRule.AlwaysInclude, LanguageNames.Stark);
            BindToOption(Include_snippets_when_question_Tab_is_typed_after_an_identifier, CompletionOptions.SnippetsBehavior, SnippetsRule.IncludeAfterTypingIdentifierQuestionTab, LanguageNames.Stark);

            BindToOption(Never_add_new_line_on_enter, CompletionOptions.EnterKeyBehavior, EnterKeyRule.Never, LanguageNames.Stark);
            BindToOption(Only_add_new_line_on_enter_with_whole_word, CompletionOptions.EnterKeyBehavior, EnterKeyRule.AfterFullyTypedWord, LanguageNames.Stark);
            BindToOption(Always_add_new_line_on_enter, CompletionOptions.EnterKeyBehavior, EnterKeyRule.Always, LanguageNames.Stark);

            BindToOption(Show_name_suggestions, CompletionOptions.ShowNameSuggestions, LanguageNames.Stark);
        }

        private void Show_completion_list_after_a_character_is_typed_Checked(object sender, RoutedEventArgs e)
        {
            Show_completion_list_after_a_character_is_deleted.IsEnabled = Show_completion_list_after_a_character_is_typed.IsChecked == true;
        }

        private void Show_completion_list_after_a_character_is_typed_Unchecked(object sender, RoutedEventArgs e)
        {
            Show_completion_list_after_a_character_is_deleted.IsEnabled = Show_completion_list_after_a_character_is_typed.IsChecked == true;
            Show_completion_list_after_a_character_is_deleted.IsChecked = false;
            Show_completion_list_after_a_character_is_deleted_Unchecked(sender, e);
        }

        private void Show_completion_list_after_a_character_is_deleted_Checked(object sender, RoutedEventArgs e)
        {
            this.OptionService.SetOptions(
                this.OptionService.GetOptions().WithChangedOption(
                    CompletionOptions.TriggerOnDeletion, LanguageNames.Stark, value: true));
        }

        private void Show_completion_list_after_a_character_is_deleted_Unchecked(object sender, RoutedEventArgs e)
        {
            this.OptionService.SetOptions(
                this.OptionService.GetOptions().WithChangedOption(
                    CompletionOptions.TriggerOnDeletion, LanguageNames.Stark, value: false));
        }
    }
}
