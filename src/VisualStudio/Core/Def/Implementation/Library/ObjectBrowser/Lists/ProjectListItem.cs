﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using StarkPlatform.CodeAnalysis;
using Microsoft.VisualStudio.Language.Intellisense;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.Library.ObjectBrowser.Lists
{
    internal class ProjectListItem : ObjectListItem
    {
        private readonly string _displayText;

        public ProjectListItem(Project project)
            : base(project.Id, GetProjectGlyph(project))
        {
            _displayText = project.GetProjectDisplayName();
        }

        private static StandardGlyphGroup GetProjectGlyph(Project project)
        {
            switch (project.Language)
            {
                case LanguageNames.Stark:
                    return StandardGlyphGroup.GlyphCoolProject;
                default:
                    throw new InvalidOperationException("Unsupported language: " + project.Language);
            }
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
