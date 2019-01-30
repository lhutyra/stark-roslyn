﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis;
using Microsoft.VisualStudio.Language.Intellisense;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.Library.ObjectBrowser.Lists
{
    internal class FolderListItem : ObjectListItem
    {
        private readonly string _displayText;

        public FolderListItem(ProjectId projectId, string displayText)
            : base(projectId, StandardGlyphGroup.GlyphClosedFolder)
        {
            _displayText = displayText;
        }

        public override string DisplayText
        {
            get { return _displayText; }
        }

        public override string FullNameText
        {
            get { return _displayText; }
        }

        public override string SearchText
        {
            get { return _displayText; }
        }
    }
}
