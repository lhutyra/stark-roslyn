﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using StarkPlatform.CodeAnalysis;
using Microsoft.VisualStudio.ComponentModelHost;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.Library.ObjectBrowser;
using Microsoft.VisualStudio.Shell.Interop;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.ObjectBrowser
{
    internal class ObjectBrowserLibraryManager : AbstractObjectBrowserLibraryManager
    {
        public ObjectBrowserLibraryManager(IServiceProvider serviceProvider, IComponentModel componentModel, VisualStudioWorkspace workspace)
            : base(LanguageNames.Stark, Guids.StarkLibraryId, __SymbolToolLanguage.SymbolToolLanguage_CSharp, serviceProvider, componentModel, workspace)
        {
        }

        internal override AbstractDescriptionBuilder CreateDescriptionBuilder(
            IVsObjectBrowserDescription3 description,
            ObjectListItem listItem,
            Project project)
        {
            return new DescriptionBuilder(description, this, listItem, project);
        }

        internal override AbstractListItemFactory CreateListItemFactory()
        {
            return new ListItemFactory();
        }
    }
}
