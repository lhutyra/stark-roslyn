﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Diagnostics;
using StarkPlatform.CodeAnalysis.Editor.Shared.Extensions;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Imaging.Interop;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.Utilities;
using Roslyn.Utilities;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.SolutionExplorer
{
    using Workspace = StarkPlatform.CodeAnalysis.Workspace;

    internal sealed partial class LegacyDiagnosticItem : BaseDiagnosticItem
    {
        private readonly AnalyzerItem _analyzerItem;
        private readonly IContextMenuController _contextMenuController;

        public LegacyDiagnosticItem(AnalyzerItem analyzerItem, DiagnosticDescriptor descriptor, ReportDiagnostic effectiveSeverity, IContextMenuController contextMenuController)
            : base(descriptor, effectiveSeverity)
        {
            _analyzerItem = analyzerItem;

            _contextMenuController = contextMenuController;
        }

        protected override Workspace Workspace
        {
            get { return _analyzerItem.AnalyzersFolder.Workspace; }
        }

        public override ProjectId ProjectId
        {
            get { return _analyzerItem.AnalyzersFolder.ProjectId; }
        }

        protected override AnalyzerReference AnalyzerReference
        {
            get { return _analyzerItem.AnalyzerReference; }
        }

        public override IContextMenuController ContextMenuController
        {
            get
            {
                return _contextMenuController;
            }
        }
    }
}
