// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.Options;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.Options.Formatting
{
    [GuidAttribute(Guids.StarkOptionPageFormattingGeneralIdString)]
    internal class FormattingOptionPage : AbstractOptionPage
    {
        private FormattingOptionPageControl _optionPageControl;

        protected override AbstractOptionPageControl CreateOptionPage(IServiceProvider serviceProvider)
        {
            _optionPageControl = new FormattingOptionPageControl(serviceProvider);
            return _optionPageControl;
        }
    }
}
