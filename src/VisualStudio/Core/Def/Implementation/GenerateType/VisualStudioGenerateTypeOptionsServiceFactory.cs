// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Composition;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.GeneratedCodeRecognition;
using StarkPlatform.CodeAnalysis.GenerateType;
using StarkPlatform.CodeAnalysis.Host;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.CodeAnalysis.LanguageServices;
using StarkPlatform.CodeAnalysis.Notification;
using StarkPlatform.CodeAnalysis.ProjectManagement;
using Microsoft.VisualStudio.Language.Intellisense;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.GenerateType
{
    [ExportWorkspaceServiceFactory(typeof(IGenerateTypeOptionsService), ServiceLayer.Host), Shared]
    internal class VisualStudioGenerateTypeOptionsServiceFactory : IWorkspaceServiceFactory
    {
        public IWorkspaceService CreateService(HostWorkspaceServices workspaceServices)
        {
            return new VisualStudioGenerateTypeOptionsService();
        }

        private class VisualStudioGenerateTypeOptionsService : IGenerateTypeOptionsService
        {
            private bool _isNewFile = false;
            private string _accessSelectString = "";
            private string _typeKindSelectString = "";

            public GenerateTypeOptionsResult GetGenerateTypeOptions(
                string typeName,
                GenerateTypeDialogOptions generateTypeDialogOptions,
                Document document,
                INotificationService notificationService,
                IProjectManagementService projectManagementService,
                ISyntaxFactsService syntaxFactsService)
            {
                var viewModel = new GenerateTypeDialogViewModel(
                    document,
                    notificationService,
                    projectManagementService,
                    syntaxFactsService,
                    generateTypeDialogOptions,
                    typeName,
                    document.Project.Language == LanguageNames.Stark ? ".cs" : ".vb",
                    _isNewFile,
                    _accessSelectString,
                    _typeKindSelectString);

                var dialog = new GenerateTypeDialog(viewModel);
                var result = dialog.ShowModal();

                if (result.HasValue && result.Value)
                {
                    // Retain choice
                    _isNewFile = viewModel.IsNewFile;
                    _accessSelectString = viewModel.SelectedAccessibilityString;
                    _typeKindSelectString = viewModel.SelectedTypeKindString;

                    var defaultNamespace = projectManagementService.GetDefaultNamespace(viewModel.SelectedProject, viewModel.SelectedProject?.Solution.Workspace);

                    return new GenerateTypeOptionsResult(
                        accessibility: viewModel.SelectedAccessibility,
                        typeKind: viewModel.SelectedTypeKind,
                        typeName: viewModel.TypeName,
                        project: viewModel.SelectedProject,
                        isNewFile: viewModel.IsNewFile,
                        newFileName: viewModel.FileName.Trim(),
                        folders: viewModel.Folders,
                        fullFilePath: viewModel.FullFilePath,
                        existingDocument: viewModel.SelectedDocument,
                        defaultNamespace: defaultNamespace,
                        areFoldersValidIdentifiers: viewModel.AreFoldersValidIdentifiers);
                }
                else
                {
                    return GenerateTypeOptionsResult.Cancelled;
                }
            }
        }
    }
}
