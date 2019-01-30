// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.InteropServices;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Stark.CodeStyle;
using StarkPlatform.CodeAnalysis.Stark.Formatting;
using StarkPlatform.CodeAnalysis.Options;
using StarkPlatform.CodeAnalysis.PooledObjects;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.Options;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.Options.Formatting
{
    [Guid(Guids.StarkOptionPageCodeStyleIdString)]
    internal class CodeStylePage : AbstractOptionPage
    {
        protected override AbstractOptionPageControl CreateOptionPage(IServiceProvider serviceProvider)
        {
            return new GridOptionPreviewControl(
                serviceProvider,
                (o, s) => new StyleViewModel(o, s),
                GetEditorConfigOptions(),
                LanguageNames.Stark);
        }

        private static ImmutableArray<(string feature, ImmutableArray<IOption> options)> GetEditorConfigOptions()
        {
            var builder = ArrayBuilder<(string, ImmutableArray<IOption>)>.GetInstance();
            builder.AddRange(GridOptionPreviewControl.GetLanguageAgnosticEditorConfigOptions());
            builder.Add((CSharpVSResources.CSharp_Coding_Conventions, CSharpCodeStyleOptions.AllOptions));
            builder.Add((CSharpVSResources.CSharp_Formatting_Rules, CSharpFormattingOptions.AllOptions));
            return builder.ToImmutableAndFree();
        }

        internal readonly struct TestAccessor
        {
            internal static ImmutableArray<(string feature, ImmutableArray<IOption> options)> GetEditorConfigOptions()
            {
                return CodeStylePage.GetEditorConfigOptions();
            }
        }
    }
}
