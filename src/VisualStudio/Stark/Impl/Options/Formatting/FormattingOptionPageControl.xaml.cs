// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.CodeCleanup;
using StarkPlatform.CodeAnalysis.Stark;
using StarkPlatform.CodeAnalysis.Editor.Shared.Options;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.Options;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.Options
{
    /// <summary>
    /// Interaction logic for FormattingOptionPageControl.xaml
    /// </summary>
    internal partial class FormattingOptionPageControl : AbstractOptionPageControl
    {
        public FormattingOptionPageControl(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            InitializeComponent();

            FormatWhenTypingCheckBox.Content = CSharpVSResources.Automatically_format_when_typing;
            FormatOnSemicolonCheckBox.Content = CSharpVSResources.Automatically_format_statement_on_semicolon;
            FormatOnCloseBraceCheckBox.Content = CSharpVSResources.Automatically_format_block_on_close_brace;
            FormatOnReturnCheckBox.Content = CSharpVSResources.Automatically_format_on_return;
            FormatOnPasteCheckBox.Content = CSharpVSResources.Automatically_format_on_paste;

            BindToOption(FormatWhenTypingCheckBox, FeatureOnOffOptions.AutoFormattingOnTyping, LanguageNames.Stark);
            BindToOption(FormatOnCloseBraceCheckBox, FeatureOnOffOptions.AutoFormattingOnCloseBrace, LanguageNames.Stark);
            BindToOption(FormatOnSemicolonCheckBox, FeatureOnOffOptions.AutoFormattingOnSemicolon, LanguageNames.Stark);
            BindToOption(FormatOnReturnCheckBox, FeatureOnOffOptions.AutoFormattingOnReturn, LanguageNames.Stark);
            BindToOption(FormatOnPasteCheckBox, FeatureOnOffOptions.FormatOnPaste, LanguageNames.Stark);
        }
    }
}
