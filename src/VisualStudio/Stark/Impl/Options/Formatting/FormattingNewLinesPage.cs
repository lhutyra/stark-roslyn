// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.Options;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.Options.Formatting
{
    internal class FormattingNewLinesPage : AbstractOptionPage
    {
        public FormattingNewLinesPage()
        {
        }

        protected override AbstractOptionPageControl CreateOptionPage(IServiceProvider serviceProvider)
        {
            return new OptionPreviewControl(serviceProvider, (o, s) => new NewLinesViewModel(o, s));
        }
    }
}
