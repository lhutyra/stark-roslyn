﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Diagnostics;
using StarkPlatform.CodeAnalysis.ErrorReporting;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.ProjectSystem;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.ProjectSystem.Extensions;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.Venus;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using Roslyn.Utilities;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.TaskList
{
    internal class ProjectExternalErrorReporter : IVsReportExternalErrors, IVsLanguageServiceBuildErrorReporter2
    {
        internal static readonly IReadOnlyList<string> CustomTags = ImmutableArray.Create(WellKnownDiagnosticTags.Telemetry);
        internal static readonly IReadOnlyList<string> CompilerDiagnosticCustomTags = ImmutableArray.Create(WellKnownDiagnosticTags.Compiler, WellKnownDiagnosticTags.Telemetry);

        private readonly ProjectId _projectId;
        private readonly string _errorCodePrefix;

        private readonly VisualStudioWorkspace _workspace;
        private readonly ExternalErrorDiagnosticUpdateSource _diagnosticProvider;

        public ProjectExternalErrorReporter(ProjectId projectId, string errorCodePrefix, IServiceProvider serviceProvider)
            : this(projectId, errorCodePrefix, serviceProvider.GetMefService<VisualStudioWorkspace>(), serviceProvider.GetMefService<ExternalErrorDiagnosticUpdateSource>())
        {
        }

        public ProjectExternalErrorReporter(ProjectId projectId, string errorCodePrefix, VisualStudioWorkspace workspace, ExternalErrorDiagnosticUpdateSource diagnosticProvider)
        {
            Debug.Assert(workspace != null);

            // TODO: re-enable this assert; right now it'll fail in unit tests
            // Debug.Assert(diagnosticProvider != null);

            _projectId = projectId;
            _errorCodePrefix = errorCodePrefix;
            _workspace = workspace;
            _diagnosticProvider = diagnosticProvider;
        }

        private bool CanHandle(string errorId)
        {
            // make sure we have error id, otherwise, we simple don't support
            // this error
            if (errorId == null)
            {
                return false;
            }

            // we accept all compiler diagnostics
            if (errorId.StartsWith(_errorCodePrefix))
            {
                return true;
            }

            return _diagnosticProvider.SupportedDiagnosticId(_projectId, errorId);
        }

        public int AddNewErrors(IVsEnumExternalErrors pErrors)
        {
            var projectErrors = new HashSet<DiagnosticData>();
            var documentErrorsMap = new Dictionary<DocumentId, HashSet<DiagnosticData>>();

            var errors = new ExternalError[1];
            while (pErrors.Next(1, errors, out var fetched) == VSConstants.S_OK && fetched == 1)
            {
                var error = errors[0];

                DiagnosticData diagnostic;
                if (error.bstrFileName != null)
                {
                    diagnostic = CreateDocumentDiagnosticItem(error);
                    if (diagnostic != null)
                    {
                        var diagnostics = documentErrorsMap.GetOrAdd(diagnostic.DocumentId, _ => new HashSet<DiagnosticData>());
                        diagnostics.Add(diagnostic);
                        continue;
                    }

                    projectErrors.Add(CreateProjectDiagnosticItem(error));
                }
                else
                {
                    projectErrors.Add(CreateProjectDiagnosticItem(error));
                }
            }

            _diagnosticProvider.AddNewErrors(_projectId, projectErrors, documentErrorsMap);
            return VSConstants.S_OK;
        }

        public int ClearAllErrors()
        {
            _diagnosticProvider.ClearErrors(_projectId);
            return VSConstants.S_OK;
        }

        public int GetErrors(out IVsEnumExternalErrors pErrors)
        {
            pErrors = null;
            Debug.Fail("This is not implemented, because no one called it.");
            return VSConstants.E_NOTIMPL;
        }

        private DiagnosticData CreateProjectDiagnosticItem(ExternalError error)
        {
            return GetDiagnosticData(error);
        }

        private DocumentId TryGetDocumentId(string filePath)
        {
            return _workspace.CurrentSolution.GetDocumentIdsWithFilePath(filePath)
                             .Where(f => f.ProjectId == _projectId)
                             .FirstOrDefault();
        }

        private DiagnosticData CreateDocumentDiagnosticItem(ExternalError error)
        {
            var documentId = TryGetDocumentId(error.bstrFileName);
            if (documentId == null)
            {
                return null;
            }

            var line = error.iLine;
            var column = error.iCol;
            /* TODO: make work again
            if (hostDocument is ContainedDocument containedDocument)
            {
                var span = new VsTextSpan
                {
                    iStartLine = line,
                    iStartIndex = column,
                    iEndLine = line,
                    iEndIndex = column,
                };

                var spans = new VsTextSpan[1];
                Marshal.ThrowExceptionForHR(containedDocument.ContainedLanguage.BufferCoordinator.MapPrimaryToSecondarySpan(
                    span,
                    spans));

                line = spans[0].iStartLine;
                column = spans[0].iStartIndex;
            }
            */

            return GetDiagnosticData(error, documentId, line, column);
        }

        public int ReportError(string bstrErrorMessage, string bstrErrorId, [ComAliasName("VsShell.VSTASKPRIORITY")]VSTASKPRIORITY nPriority, int iLine, int iColumn, string bstrFileName)
        {
            ReportError2(bstrErrorMessage, bstrErrorId, nPriority, iLine, iColumn, iLine, iColumn, bstrFileName);
            return VSConstants.S_OK;
        }

        // TODO: Use PreserveSig instead of throwing these exceptions for common cases.
        public void ReportError2(string bstrErrorMessage, string bstrErrorId, [ComAliasName("VsShell.VSTASKPRIORITY")]VSTASKPRIORITY nPriority, int iStartLine, int iStartColumn, int iEndLine, int iEndColumn, string bstrFileName)
        {
            // first we check whether given error is something we can take care.
            if (!CanHandle(bstrErrorId))
            {
                // it is not, let project system takes care.
                throw new NotImplementedException();
            }

            if ((iEndLine >= 0 && iEndColumn >= 0) &&
               ((iEndLine < iStartLine) ||
                (iEndLine == iStartLine && iEndColumn < iStartColumn)))
            {
                throw new ArgumentException(ServicesVSResources.End_position_must_be_start_position);
            }

            var priority = (VSTASKPRIORITY)nPriority;
            DiagnosticSeverity severity;
            switch (priority)
            {
                case VSTASKPRIORITY.TP_HIGH:
                    severity = DiagnosticSeverity.Error;
                    break;
                case VSTASKPRIORITY.TP_NORMAL:
                    severity = DiagnosticSeverity.Warning;
                    break;
                case VSTASKPRIORITY.TP_LOW:
                    severity = DiagnosticSeverity.Info;
                    break;
                default:
                    throw new ArgumentException(ServicesVSResources.Not_a_valid_value, nameof(nPriority));
            }

            if (bstrFileName == null || iStartLine < 0 || iStartColumn < 0)
            {
                // we now takes care of errors that is not belong to file as well.
                var projectDiagnostic = GetDiagnosticData(
                    null, bstrErrorId, bstrErrorMessage, severity,
                    null, 0, 0, 0, 0,
                    bstrFileName, 0, 0, 0, 0);

                _diagnosticProvider.AddNewErrors(_projectId, projectDiagnostic);
                return;
            }

            var documentId = TryGetDocumentId(bstrFileName);
            if (documentId == null)
            {
                var projectDiagnostic = GetDiagnosticData(
                    null, bstrErrorId, bstrErrorMessage, severity,
                    null, iStartLine, iStartColumn, iEndLine, iEndColumn,
                    bstrFileName, iStartLine, iStartColumn, iEndLine, iEndColumn);

                _diagnosticProvider.AddNewErrors(_projectId, projectDiagnostic);
                return;
            }

            var diagnostic = GetDiagnosticData(
                documentId, bstrErrorId, bstrErrorMessage, severity,
                null, iStartLine, iStartColumn, iEndLine, iEndColumn,
                bstrFileName, iStartLine, iStartColumn, iEndLine, iEndColumn);

            _diagnosticProvider.AddNewErrors(documentId, diagnostic);
        }

        public int ClearErrors()
        {
            _diagnosticProvider.ClearErrors(_projectId);
            return VSConstants.S_OK;
        }

        private string GetErrorId(ExternalError error)
        {
            return string.Format("{0}{1:0000}", _errorCodePrefix, error.iErrorID);
        }

        private static int GetWarningLevel(DiagnosticSeverity severity)
        {
            return severity == DiagnosticSeverity.Error ? 0 : 1;
        }

        private static DiagnosticSeverity GetDiagnosticSeverity(ExternalError error)
        {
            return error.fError != 0 ? DiagnosticSeverity.Error : DiagnosticSeverity.Warning;
        }

        private DiagnosticData GetDiagnosticData(
            ExternalError error, DocumentId id = null, int line = 0, int column = 0)
        {
            if (id != null)
            {
                // save error line/column (surface buffer location) as mapped line/column so that we can display
                // right location on closed Venus file.
                return GetDiagnosticData(
                    id, GetErrorId(error), error.bstrText, GetDiagnosticSeverity(error),
                    null, error.iLine, error.iCol, error.iLine, error.iCol, error.bstrFileName, line, column, line, column);
            }

            return GetDiagnosticData(
                id, GetErrorId(error), error.bstrText, GetDiagnosticSeverity(error), null, 0, 0, 0, 0, null, 0, 0, 0, 0);
        }

        private static bool IsCompilerDiagnostic(string errorId)
        {
            if (!string.IsNullOrEmpty(errorId) && errorId.Length > 2)
            {
                var prefix = errorId.Substring(0, 2);
                if (prefix.Equals("CS", StringComparison.OrdinalIgnoreCase) || prefix.Equals("BC", StringComparison.OrdinalIgnoreCase))
                {
                    var suffix = errorId.Substring(2);
                    return int.TryParse(suffix, out var id);
                }
            }

            return false;
        }

        private static IReadOnlyList<string> GetCustomTags(string errorId)
        {
            return IsCompilerDiagnostic(errorId) ? CompilerDiagnosticCustomTags : CustomTags;
        }

        private DiagnosticData GetDiagnosticData(
            DocumentId id, string errorId, string message, DiagnosticSeverity severity,
            string mappedFilePath, int mappedStartLine, int mappedStartColumn, int mappedEndLine, int mappedEndColumn,
            string originalFilePath, int originalStartLine, int originalStartColumn, int originalEndLine, int originalEndColumn)
        {
            return new DiagnosticData(
                id: errorId,
                category: WellKnownDiagnosticTags.Build,
                message: message,
                title: message,
                enuMessageForBingSearch: message, // Unfortunately, there is no way to get ENU text for this since this is an external error.
                severity: severity,
                defaultSeverity: severity,
                isEnabledByDefault: true,
                warningLevel: GetWarningLevel(severity),
                customTags: GetCustomTags(errorId),
                properties: DiagnosticData.PropertiesForBuildDiagnostic,
                workspace: _workspace,
                projectId: _projectId,
                location: new DiagnosticDataLocation(id,
                    sourceSpan: null,
                    originalFilePath: originalFilePath,
                    originalStartLine: originalStartLine,
                    originalStartColumn: originalStartColumn,
                    originalEndLine: originalEndLine,
                    originalEndColumn: originalEndColumn,
                    mappedFilePath: mappedFilePath,
                    mappedStartLine: mappedStartLine,
                    mappedStartColumn: mappedStartColumn,
                    mappedEndLine: mappedEndLine,
                    mappedEndColumn: mappedEndColumn));
        }
    }
}
