﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Composition;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.ChangeSignature;
using StarkPlatform.CodeAnalysis.Editor.Shared.Utilities;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.CodeAnalysis.Notification;
using Microsoft.VisualStudio.Text.Classification;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.ChangeSignature
{
    [ExportWorkspaceService(typeof(IChangeSignatureOptionsService), ServiceLayer.Host), Shared]
    internal class VisualStudioChangeSignatureOptionsService : IChangeSignatureOptionsService
    {
        private readonly IClassificationFormatMap _classificationFormatMap;
        private readonly ClassificationTypeMap _classificationTypeMap;

        [ImportingConstructor]
        public VisualStudioChangeSignatureOptionsService(IClassificationFormatMapService classificationFormatMapService, ClassificationTypeMap classificationTypeMap)
        {
            _classificationFormatMap = classificationFormatMapService.GetClassificationFormatMap("tooltip");
            _classificationTypeMap = classificationTypeMap;
        }

        public ChangeSignatureOptionsResult GetChangeSignatureOptions(ISymbol symbol, ParameterConfiguration parameters, INotificationService notificationService)
        {
            var viewModel = new ChangeSignatureDialogViewModel(notificationService, parameters, symbol, _classificationFormatMap, _classificationTypeMap);

            var dialog = new ChangeSignatureDialog(viewModel);
            var result = dialog.ShowModal();

            if (result.HasValue && result.Value)
            {
                return new ChangeSignatureOptionsResult { IsCancelled = false, UpdatedSignature = new SignatureChange(parameters, viewModel.GetParameterConfiguration()), PreviewChanges = viewModel.PreviewChanges };
            }
            else
            {
                return new ChangeSignatureOptionsResult { IsCancelled = true };
            }
        }
    }
}
