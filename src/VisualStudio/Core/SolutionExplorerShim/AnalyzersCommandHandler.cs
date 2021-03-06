﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using EnvDTE;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Editor.Host;
using StarkPlatform.CodeAnalysis.Notification;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.CodeAnalysis;
using Microsoft.VisualStudio.ComponentModelHost;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.ProjectSystem;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.Utilities;
using StarkPlatform.VisualStudio.LanguageServices.SolutionExplorer;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Roslyn.Utilities;
using VSLangProj140;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.SolutionExplorer
{
    using Workspace = StarkPlatform.CodeAnalysis.Workspace;

    [Export]
    internal class AnalyzersCommandHandler : IAnalyzersCommandHandler, IVsUpdateSolutionEvents
    {
        private readonly AnalyzerItemsTracker _tracker;
        private readonly AnalyzerReferenceManager _analyzerReferenceManager;
        private readonly IServiceProvider _serviceProvider;

        private ContextMenuController _analyzerFolderContextMenuController;
        private ContextMenuController _analyzerContextMenuController;
        private ContextMenuController _diagnosticContextMenuController;

        // Analyzers folder context menu items
        private MenuCommand _addMenuItem;
        private MenuCommand _openRuleSetMenuItem;

        // Analyzer context menu items
        private MenuCommand _removeMenuItem;

        // Diagnostic context menu items
        private MenuCommand _setSeverityDefaultMenuItem;
        private MenuCommand _setSeverityErrorMenuItem;
        private MenuCommand _setSeverityWarningMenuItem;
        private MenuCommand _setSeverityInfoMenuItem;
        private MenuCommand _setSeverityHiddenMenuItem;
        private MenuCommand _setSeverityNoneMenuItem;
        private MenuCommand _openHelpLinkMenuItem;

        // Other menu items
        private MenuCommand _projectAddMenuItem;
        private MenuCommand _projectContextAddMenuItem;
        private MenuCommand _referencesContextAddMenuItem;
        private MenuCommand _setActiveRuleSetMenuItem;

        private Workspace _workspace;

        private bool _allowProjectSystemOperations = true;

        [ImportingConstructor]
        public AnalyzersCommandHandler(
            AnalyzerItemsTracker tracker,
            AnalyzerReferenceManager analyzerReferenceManager,
            [Import(typeof(SVsServiceProvider))]IServiceProvider serviceProvider)
        {
            _tracker = tracker;
            _analyzerReferenceManager = analyzerReferenceManager;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Hook up the context menu handlers.
        /// </summary>
        public void Initialize(IMenuCommandService menuCommandService)
        {
            if (menuCommandService != null)
            {
                // Analyzers folder context menu items
                _addMenuItem = AddCommandHandler(menuCommandService, ID.RoslynCommands.AddAnalyzer, AddAnalyzerHandler);
                _openRuleSetMenuItem = AddCommandHandler(menuCommandService, ID.RoslynCommands.OpenRuleSet, OpenRuleSetHandler);

                // Analyzer context menu items
                _removeMenuItem = AddCommandHandler(menuCommandService, ID.RoslynCommands.RemoveAnalyzer, RemoveAnalyzerHandler);

                // Diagnostic context menu items
                _setSeverityDefaultMenuItem = AddCommandHandler(menuCommandService, ID.RoslynCommands.SetSeverityDefault, SetSeverityHandler);
                _setSeverityErrorMenuItem = AddCommandHandler(menuCommandService, ID.RoslynCommands.SetSeverityError, SetSeverityHandler);
                _setSeverityWarningMenuItem = AddCommandHandler(menuCommandService, ID.RoslynCommands.SetSeverityWarning, SetSeverityHandler);
                _setSeverityInfoMenuItem = AddCommandHandler(menuCommandService, ID.RoslynCommands.SetSeverityInfo, SetSeverityHandler);
                _setSeverityHiddenMenuItem = AddCommandHandler(menuCommandService, ID.RoslynCommands.SetSeverityHidden, SetSeverityHandler);
                _setSeverityNoneMenuItem = AddCommandHandler(menuCommandService, ID.RoslynCommands.SetSeverityNone, SetSeverityHandler);
                _openHelpLinkMenuItem = AddCommandHandler(menuCommandService, ID.RoslynCommands.OpenDiagnosticHelpLink, OpenDiagnosticHelpLinkHandler);

                // Other menu items
                _projectAddMenuItem = AddCommandHandler(menuCommandService, ID.RoslynCommands.ProjectAddAnalyzer, AddAnalyzerHandler);
                _projectContextAddMenuItem = AddCommandHandler(menuCommandService, ID.RoslynCommands.ProjectContextAddAnalyzer, AddAnalyzerHandler);
                _referencesContextAddMenuItem = AddCommandHandler(menuCommandService, ID.RoslynCommands.ReferencesContextAddAnalyzer, AddAnalyzerHandler);
                _setActiveRuleSetMenuItem = AddCommandHandler(menuCommandService, ID.RoslynCommands.SetActiveRuleSet, SetActiveRuleSetHandler);

                UpdateOtherMenuItemsVisibility();

                if (_tracker != null)
                {
                    _tracker.SelectedHierarchyItemChanged += SelectedHierarchyItemChangedHandler;
                }

                var buildManager = (IVsSolutionBuildManager)_serviceProvider.GetService(typeof(SVsSolutionBuildManager));
                buildManager.AdviseUpdateSolutionEvents(this, out var cookie);
            }
        }

        public IContextMenuController AnalyzerFolderContextMenuController
        {
            get
            {
                if (_analyzerFolderContextMenuController == null)
                {
                    _analyzerFolderContextMenuController = new ContextMenuController(
                        ID.RoslynCommands.AnalyzerFolderContextMenu,
                        ShouldShowAnalyzerFolderContextMenu,
                        UpdateAnalyzerFolderContextMenu);
                }

                return _analyzerFolderContextMenuController;
            }
        }

        private bool ShouldShowAnalyzerFolderContextMenu(IEnumerable<object> items)
        {
            return items.Count() == 1;
        }

        private void UpdateAnalyzerFolderContextMenu()
        {
            if (_addMenuItem != null)
            {
                _addMenuItem.Visible = SelectedProjectSupportsAnalyzers();
                _addMenuItem.Enabled = _allowProjectSystemOperations;
            }
        }

        public IContextMenuController AnalyzerContextMenuController
        {
            get
            {
                if (_analyzerContextMenuController == null)
                {
                    _analyzerContextMenuController = new ContextMenuController(
                        ID.RoslynCommands.AnalyzerContextMenu,
                        ShouldShowAnalyzerContextMenu,
                        UpdateAnalyzerContextMenu);
                }

                return _analyzerContextMenuController;
            }
        }

        private bool ShouldShowAnalyzerContextMenu(IEnumerable<object> items)
        {
            return items.All(item => item is AnalyzerItem);
        }

        private void UpdateAnalyzerContextMenu()
        {
            _removeMenuItem.Enabled = _allowProjectSystemOperations;
        }

        public IContextMenuController DiagnosticContextMenuController
        {
            get
            {
                if (_diagnosticContextMenuController == null)
                {
                    _diagnosticContextMenuController = new ContextMenuController(
                        ID.RoslynCommands.DiagnosticContextMenu,
                        ShouldShowDiagnosticContextMenu,
                        UpdateDiagnosticContextMenu);
                }

                return _diagnosticContextMenuController;
            }
        }

        private bool ShouldShowDiagnosticContextMenu(IEnumerable<object> items)
        {
            return items.All(item => item is BaseDiagnosticItem);
        }

        private void UpdateDiagnosticContextMenu()
        {
            UpdateSeverityMenuItemsChecked();
            UpdateSeverityMenuItemsEnabled();
            UpdateOpenHelpLinkMenuItemVisibility();
        }

        private MenuCommand AddCommandHandler(IMenuCommandService menuCommandService, int roslynCommand, EventHandler handler)
        {
            var commandID = new CommandID(Guids.RoslynGroupId, roslynCommand);
            var menuCommand = new MenuCommand(handler, commandID);
            menuCommandService.AddCommand(menuCommand);

            return menuCommand;
        }

        private void SelectedHierarchyItemChangedHandler(object sender, EventArgs e)
        {
            UpdateOtherMenuItemsVisibility();
        }

        private void UpdateOtherMenuItemsVisibility()
        {
            bool selectedProjectSupportsAnalyzers = SelectedProjectSupportsAnalyzers();
            _projectAddMenuItem.Visible = selectedProjectSupportsAnalyzers;
            _projectContextAddMenuItem.Visible = selectedProjectSupportsAnalyzers && _tracker.SelectedItemId == VSConstants.VSITEMID_ROOT;
            _referencesContextAddMenuItem.Visible = selectedProjectSupportsAnalyzers;
            _setActiveRuleSetMenuItem.Visible = selectedProjectSupportsAnalyzers &&
                                                _tracker.SelectedHierarchy.TryGetItemName(_tracker.SelectedItemId, out var itemName) &&
                                                Path.GetExtension(itemName).Equals(".ruleset", StringComparison.OrdinalIgnoreCase);
        }

        private void UpdateOtherMenuItemsEnabled()
        {
            _projectAddMenuItem.Enabled = _allowProjectSystemOperations;
            _projectContextAddMenuItem.Enabled = _allowProjectSystemOperations;
            _referencesContextAddMenuItem.Enabled = _allowProjectSystemOperations;
            _removeMenuItem.Enabled = _allowProjectSystemOperations;
        }

        private void UpdateOpenHelpLinkMenuItemVisibility()
        {
            _openHelpLinkMenuItem.Visible = _tracker.SelectedDiagnosticItems.Length == 1 &&
                                            _tracker.SelectedDiagnosticItems[0].GetHelpLink() != null;
        }

        private void UpdateSeverityMenuItemsChecked()
        {
            _setSeverityDefaultMenuItem.Checked = false;
            _setSeverityErrorMenuItem.Checked = false;
            _setSeverityWarningMenuItem.Checked = false;
            _setSeverityInfoMenuItem.Checked = false;
            _setSeverityHiddenMenuItem.Checked = false;
            _setSeverityNoneMenuItem.Checked = false;

            var workspace = TryGetWorkspace() as VisualStudioWorkspaceImpl;
            if (workspace == null)
            {
                return;
            }

            HashSet<ReportDiagnostic> selectedItemSeverities = new HashSet<ReportDiagnostic>();

            var groups = _tracker.SelectedDiagnosticItems.GroupBy(item => item.ProjectId);

            foreach (var group in groups)
            {
                // TODO: move this off GetHostProject (https://devdiv.visualstudio.com/DevDiv/_workitems/edit/698029)
#pragma warning disable CS0618 // Type or member is obsolete
                var project = (AbstractProject)workspace.GetHostProject(group.Key);
#pragma warning restore CS0618 // Type or member is obsolete
                IRuleSetFile ruleSet = project.RuleSetFile?.Target;

                if (ruleSet != null)
                {
                    var specificOptions = ruleSet.GetSpecificDiagnosticOptions();

                    foreach (var diagnosticItem in group)
                    {
                        if (specificOptions.TryGetValue(diagnosticItem.Descriptor.Id, out var ruleSetSeverity))
                        {
                            selectedItemSeverities.Add(ruleSetSeverity);
                        }
                        else
                        {
                            // The rule has no setting.
                            selectedItemSeverities.Add(ReportDiagnostic.Default);
                        }
                    }
                }
            }

            if (selectedItemSeverities.Count != 1)
            {
                return;
            }

            switch (selectedItemSeverities.Single())
            {
                case ReportDiagnostic.Default:
                    _setSeverityDefaultMenuItem.Checked = true;
                    break;
                case ReportDiagnostic.Error:
                    _setSeverityErrorMenuItem.Checked = true;
                    break;
                case ReportDiagnostic.Warn:
                    _setSeverityWarningMenuItem.Checked = true;
                    break;
                case ReportDiagnostic.Info:
                    _setSeverityInfoMenuItem.Checked = true;
                    break;
                case ReportDiagnostic.Hidden:
                    _setSeverityHiddenMenuItem.Checked = true;
                    break;
                case ReportDiagnostic.Suppress:
                    _setSeverityNoneMenuItem.Checked = true;
                    break;
                default:
                    break;
            }
        }

        private bool AnyDiagnosticsWithSeverity(ReportDiagnostic severity)
        {
            return _tracker.SelectedDiagnosticItems.Any(item => item.EffectiveSeverity == severity);
        }

        private void UpdateSeverityMenuItemsEnabled()
        {
            bool configurable = !_tracker.SelectedDiagnosticItems.Any(item => item.Descriptor.CustomTags.Contains(WellKnownDiagnosticTags.NotConfigurable));

            _setSeverityDefaultMenuItem.Enabled = configurable;
            _setSeverityErrorMenuItem.Enabled = configurable;
            _setSeverityWarningMenuItem.Enabled = configurable;
            _setSeverityInfoMenuItem.Enabled = configurable;
            _setSeverityHiddenMenuItem.Enabled = configurable;
            _setSeverityNoneMenuItem.Enabled = configurable;
        }

        private bool SelectedProjectSupportsAnalyzers()
        {
            return _tracker != null &&
                   _tracker.SelectedHierarchy != null &&
                   _tracker.SelectedHierarchy.TryGetProject(out var project) &&
                   project.Object is VSProject3;
        }

        /// <summary>
        /// Handler for "Add Analyzer..." context menu on Analyzers folder node.
        /// </summary>
        internal void AddAnalyzerHandler(object sender, EventArgs args)
        {
            if (_analyzerReferenceManager != null)
            {
                _analyzerReferenceManager.ShowDialog();
            }
        }

        /// <summary>
        /// Handler for "Remove" context menu on individual Analyzer items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        internal void RemoveAnalyzerHandler(object sender, EventArgs args)
        {
            foreach (var item in _tracker.SelectedAnalyzerItems)
            {
                item.Remove();
            }
        }

        internal void OpenRuleSetHandler(object sender, EventArgs args)
        {
            if (_tracker.SelectedFolder != null &&
                _serviceProvider != null)
            {
                var workspace = _tracker.SelectedFolder.Workspace as VisualStudioWorkspaceImpl;
                var projectId = _tracker.SelectedFolder.ProjectId;
                if (workspace != null)
                {
                    // TODO: move this off GetHostProject (https://devdiv.visualstudio.com/DevDiv/_workitems/edit/698029)
#pragma warning disable CS0618 // Type or member is obsolete
                    var project = workspace.GetHostProject(projectId);
#pragma warning restore CS0618 // Type or member is obsolete
                    if (project == null)
                    {
                        SendUnableToOpenRuleSetNotification(workspace, string.Format(SolutionExplorerShim.Could_not_find_project_0, projectId));
                        return;
                    }

                    if (project.RuleSetFile == null)
                    {
                        SendUnableToOpenRuleSetNotification(workspace, SolutionExplorerShim.No_rule_set_file_is_specified_or_the_file_does_not_exist);
                        return;
                    }

                    try
                    {
                        EnvDTE.DTE dte = (EnvDTE.DTE)_serviceProvider.GetService(typeof(EnvDTE.DTE));
                        dte.ItemOperations.OpenFile(project.RuleSetFile.Target.FilePath);
                    }
                    catch (Exception e)
                    {
                        SendUnableToOpenRuleSetNotification(workspace, e.Message);
                    }
                }
            }
        }

        private void SetSeverityHandler(object sender, EventArgs args)
        {
            var selectedItem = (MenuCommand)sender;
            ReportDiagnostic? selectedAction = MapSelectedItemToReportDiagnostic(selectedItem);

            if (!selectedAction.HasValue)
            {
                return;
            }

            var workspace = TryGetWorkspace() as VisualStudioWorkspaceImpl;

            if (workspace == null)
            {
                return;
            }

            foreach (var selectedDiagnostic in _tracker.SelectedDiagnosticItems)
            {
                var projectId = selectedDiagnostic.ProjectId;
                // TODO: move this off GetHostProject (https://devdiv.visualstudio.com/DevDiv/_workitems/edit/698029)
#pragma warning disable CS0618 // Type or member is obsolete
                var project = workspace.GetHostProject(projectId);
#pragma warning restore CS0618 // Type or member is obsolete

                if (project == null)
                {
                    SendUnableToUpdateRuleSetNotification(workspace, string.Format(SolutionExplorerShim.Could_not_find_project_0, projectId));
                    continue;
                }

                var pathToRuleSet = project.RuleSetFile?.Target.FilePath;

                if (pathToRuleSet == null)
                {
                    SendUnableToUpdateRuleSetNotification(workspace, SolutionExplorerShim.No_rule_set_file_is_specified_or_the_file_does_not_exist);
                    continue;
                }

                try
                {
                    project.Hierarchy.TryGetProject(out var envDteProject);

                    if (SdkUiUtilities.IsBuiltInRuleSet(pathToRuleSet, _serviceProvider))
                    {
                        pathToRuleSet = CreateCopyOfRuleSetForProject(pathToRuleSet, envDteProject);
                        if (pathToRuleSet == null)
                        {
                            SendUnableToUpdateRuleSetNotification(workspace, string.Format(SolutionExplorerShim.Could_not_create_a_rule_set_for_project_0, envDteProject.Name));
                            continue;
                        }

                        var fileInfo = new FileInfo(pathToRuleSet);
                        fileInfo.IsReadOnly = false;
                    }

                    var componentModel = (IComponentModel)_serviceProvider.GetService(typeof(SComponentModel));
                    var waitIndicator = componentModel.GetService<IWaitIndicator>();
                    waitIndicator.Wait(
                        title: SolutionExplorerShim.Rule_Set,
                        message: string.Format(SolutionExplorerShim.Checking_out_0_for_editing, Path.GetFileName(pathToRuleSet)),
                        allowCancel: false,
                        action: c =>
                        {
                            if (envDteProject.DTE.SourceControl.IsItemUnderSCC(pathToRuleSet))
                            {
                                envDteProject.DTE.SourceControl.CheckOutItem(pathToRuleSet);
                            }
                        });

                    selectedDiagnostic.SetSeverity(selectedAction.Value, pathToRuleSet);
                }
                catch (Exception e)
                {
                    SendUnableToUpdateRuleSetNotification(workspace, e.Message);
                }
            }
        }

        private void OpenDiagnosticHelpLinkHandler(object sender, EventArgs e)
        {
            if (_tracker.SelectedDiagnosticItems.Length != 1)
            {
                return;
            }

            var uri = _tracker.SelectedDiagnosticItems[0].GetHelpLink();
            if (uri != null)
            {
                BrowserHelper.StartBrowser(uri);
            }
        }

        private void SetActiveRuleSetHandler(object sender, EventArgs e)
        {
            if (_tracker.SelectedHierarchy.TryGetProject(out var project) &&
                _tracker.SelectedHierarchy.TryGetCanonicalName(_tracker.SelectedItemId, out var ruleSetFileFullPath))
            {
                string projectDirectoryFullPath = Path.GetDirectoryName(project.FullName);
                string ruleSetFileRelativePath = PathUtilities.GetRelativePath(projectDirectoryFullPath, ruleSetFileFullPath);

                UpdateProjectConfigurationsToUseRuleSetFile(project, ruleSetFileRelativePath);
            }
        }

        private string CreateCopyOfRuleSetForProject(string pathToRuleSet, EnvDTE.Project envDteProject)
        {
            string fileName = GetNewRuleSetFileNameForProject(envDteProject);
            string projectDirectory = Path.GetDirectoryName(envDteProject.FullName);
            string fullFilePath = Path.Combine(projectDirectory, fileName);
            File.Copy(pathToRuleSet, fullFilePath);
            UpdateProjectConfigurationsToUseRuleSetFile(envDteProject, fileName);
            envDteProject.ProjectItems.AddFromFile(fullFilePath);

            return fullFilePath;
        }

        private void UpdateProjectConfigurationsToUseRuleSetFile(EnvDTE.Project envDteProject, string fileName)
        {
            foreach (EnvDTE.Configuration config in envDteProject.ConfigurationManager)
            {
                EnvDTE.Properties properties = config.Properties;

                try
                {
                    EnvDTE.Property codeAnalysisRuleSetFileProperty = properties?.Item("CodeAnalysisRuleSet");

                    if (codeAnalysisRuleSetFileProperty != null)
                    {
                        codeAnalysisRuleSetFileProperty.Value = fileName;
                    }
                }
                catch (ArgumentException)
                {
                    // Unfortunately the properties collection sometimes throws an ArgumentException
                    // instead of returning null if the current configuration doesn't support CodeAnalysisRuleSet.
                    // Ignore it and move on.
                }
            }
        }

        private string GetNewRuleSetFileNameForProject(EnvDTE.Project envDteProject)
        {
            string projectName = envDteProject.Name;

            HashSet<string> projectItemNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (ProjectItem item in envDteProject.ProjectItems)
            {
                projectItemNames.Add(item.Name);
            }

            string ruleSetName = projectName + ".ruleset";
            if (!projectItemNames.Contains(ruleSetName))
            {
                return ruleSetName;
            }

            for (int i = 1; i < int.MaxValue; i++)
            {
                ruleSetName = projectName + i + ".ruleset";
                if (!projectItemNames.Contains(ruleSetName))
                {
                    return ruleSetName;
                }
            }

            return null;
        }

        private static ReportDiagnostic? MapSelectedItemToReportDiagnostic(MenuCommand selectedItem)
        {
            ReportDiagnostic? selectedAction = null;

            if (selectedItem.CommandID.Guid == Guids.RoslynGroupId)
            {
                switch (selectedItem.CommandID.ID)
                {
                    case ID.RoslynCommands.SetSeverityDefault:
                        selectedAction = ReportDiagnostic.Default;
                        break;

                    case ID.RoslynCommands.SetSeverityError:
                        selectedAction = ReportDiagnostic.Error;
                        break;

                    case ID.RoslynCommands.SetSeverityWarning:
                        selectedAction = ReportDiagnostic.Warn;
                        break;

                    case ID.RoslynCommands.SetSeverityInfo:
                        selectedAction = ReportDiagnostic.Info;
                        break;

                    case ID.RoslynCommands.SetSeverityHidden:
                        selectedAction = ReportDiagnostic.Hidden;
                        break;

                    case ID.RoslynCommands.SetSeverityNone:
                        selectedAction = ReportDiagnostic.Suppress;
                        break;

                    default:
                        selectedAction = null;
                        break;
                }
            }

            return selectedAction;
        }

        private void SendUnableToOpenRuleSetNotification(Workspace workspace, string message)
        {
            SendErrorNotification(
                workspace,
                SolutionExplorerShim.The_rule_set_file_could_not_be_opened,
                message);
        }

        private void SendUnableToUpdateRuleSetNotification(Workspace workspace, string message)
        {
            SendErrorNotification(
                workspace,
                SolutionExplorerShim.The_rule_set_file_could_not_be_updated,
                message);
        }

        private void SendErrorNotification(Workspace workspace, string message1, string message2)
        {
            var notificationService = workspace.Services.GetService<INotificationService>();

            notificationService.SendNotification(message1 + Environment.NewLine + Environment.NewLine + message2, severity: NotificationSeverity.Error);
        }

        int IVsUpdateSolutionEvents.UpdateSolution_Begin(ref int pfCancelUpdate)
        {
            _allowProjectSystemOperations = false;
            UpdateOtherMenuItemsEnabled();

            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents.UpdateSolution_Done(int fSucceeded, int fModified, int fCancelCommand)
        {
            _allowProjectSystemOperations = true;
            UpdateOtherMenuItemsEnabled();

            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents.UpdateSolution_StartUpdate(ref int pfCancelUpdate)
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents.UpdateSolution_Cancel()
        {
            _allowProjectSystemOperations = true;
            UpdateOtherMenuItemsEnabled();

            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents.OnActiveProjectCfgChange(IVsHierarchy pIVsHierarchy)
        {
            return VSConstants.S_OK;
        }

        private Workspace TryGetWorkspace()
        {
            if (_workspace == null)
            {
                var componentModel = (IComponentModel)_serviceProvider.GetService(typeof(SComponentModel));
                var provider = componentModel.DefaultExportProvider.GetExportedValueOrDefault<ISolutionExplorerWorkspaceProvider>();
                if (provider != null)
                {
                    _workspace = provider.GetWorkspace();
                }
            }

            return _workspace;
        }
    }
}
