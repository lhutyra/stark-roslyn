﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.IO;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.CodeModel;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.TaskList;
using StarkPlatform.VisualStudio.LanguageServices.ProjectSystem;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.ProjectSystem.CPS
{
    [Export(typeof(IWorkspaceProjectContextFactory))]
    internal partial class CPSProjectFactory : IWorkspaceProjectContextFactory
    {
        private readonly VisualStudioProjectFactory _projectFactory;
        private readonly VisualStudioWorkspaceImpl _workspace;
        private readonly IProjectCodeModelFactory _projectCodeModelFactory;
        private readonly ExternalErrorDiagnosticUpdateSource _externalErrorDiagnosticUpdateSource;

        private static readonly ImmutableDictionary<string, string> s_projectLanguageToErrorCodePrefixMap =
            ImmutableDictionary.CreateRange(StringComparer.OrdinalIgnoreCase, new[]
            {
                new KeyValuePair<string, string> (LanguageNames.Stark, "stark"),
            });

        [ImportingConstructor]
        [Obsolete(MefConstruction.ImportingConstructorMessage, error: true)]
        public CPSProjectFactory(
            VisualStudioProjectFactory projectFactory,
            VisualStudioWorkspaceImpl workspace,
            IProjectCodeModelFactory projectCodeModelFactory,
            [Import(AllowDefault = true)] /* not present in unit tests */ ExternalErrorDiagnosticUpdateSource externalErrorDiagnosticUpdateSource)
        {
            _projectFactory = projectFactory;
            _workspace = workspace;
            _projectCodeModelFactory = projectCodeModelFactory;
            _externalErrorDiagnosticUpdateSource = externalErrorDiagnosticUpdateSource;
        }

        IWorkspaceProjectContext IWorkspaceProjectContextFactory.CreateProjectContext(
            string languageName,
            string projectUniqueName,
            string projectFilePath,
            Guid projectGuid,
            object hierarchy,
            string binOutputPath)
        {
            var visualStudioProject = CreateVisualStudioProject(languageName, projectUniqueName, projectFilePath, (IVsHierarchy)hierarchy, projectGuid);

            ProjectExternalErrorReporter errorReporter = null;

            if (s_projectLanguageToErrorCodePrefixMap.TryGetKey(languageName, out var prefix))
            {
                errorReporter = new ProjectExternalErrorReporter(visualStudioProject.Id, prefix, _workspace, _externalErrorDiagnosticUpdateSource);
            }

            return new CPSProject(visualStudioProject, _workspace, _projectCodeModelFactory, errorReporter, projectGuid, binOutputPath);
        }

        // TODO: this is a workaround. Factory has to be refactored so that all callers supply their own error reporters
        IWorkspaceProjectContext IWorkspaceProjectContextFactory.CreateProjectContext(
            string languageName,
            string projectUniqueName,
            string projectFilePath,
            Guid projectGuid,
            object hierarchy,
            string binOutputPath,
            ProjectExternalErrorReporter errorReporter)
        {
            var visualStudioProject = CreateVisualStudioProject(languageName, projectUniqueName, projectFilePath, (IVsHierarchy)hierarchy, projectGuid);
            return new CPSProject(visualStudioProject, _workspace, _projectCodeModelFactory, errorReporter, projectGuid, binOutputPath);
        }

        private VisualStudioProject CreateVisualStudioProject(string languageName, string projectUniqueName, string projectFilePath, IVsHierarchy hierarchy, Guid projectGuid)
        {
            var creationInfo = new VisualStudioProjectCreationInfo
            {
                FilePath = projectFilePath,
                Hierarchy = hierarchy,
                ProjectGuid = projectGuid,
            };

            var visualStudioProject = _projectFactory.CreateAndAddToWorkspace(projectUniqueName, languageName, creationInfo);
            return visualStudioProject;
        }
    }
}
