// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.ErrorReporting;
using StarkPlatform.CodeAnalysis.Host;
using StarkPlatform.VisualStudio.LanguageServices.CSharp.ObjectBrowser;
using StarkPlatform.VisualStudio.LanguageServices.CSharp.ProjectSystemShim;
using StarkPlatform.VisualStudio.LanguageServices.CSharp.ProjectSystemShim.Interop;
using StarkPlatform.VisualStudio.LanguageServices.Implementation;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.LanguageService;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.ProjectSystem;
using StarkPlatform.VisualStudio.LanguageServices.Utilities;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

// NOTE(DustinCa): The EditorFactory registration is in VisualStudioComponents\CSharpPackageRegistration.pkgdef.
// The reason for this is because the ProvideEditorLogicalView does not allow a name value to specified in addition to
// its GUID. This name value is used to identify untrusted logical views and link them to their physical view attributes.
// The net result is that using the attributes only causes designers to be loaded in the preview tab, even when they
// shouldn't be.

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.LanguageService
{
    // TODO(DustinCa): Put all of this in CSharpPackageRegistration.pkgdef rather than using attributes
    // (See vsproject\cool\coolpkg\pkg\VCSharp_Proj_System_Reg.pkgdef for an example).
    [Guid(Guids.StarkPackageIdString)]
    [PackageRegistration(UseManagedResourcesOnly = true)] // , AllowsBackgroundLoading = true
    [ProvideRoslynVersionRegistration(Guids.StarkPackageIdString, "Stark Language", productNameResourceID: 116, detailsResourceID: 117)]
    [ProvideLanguageExtension(typeof(StarkLanguageService), ".sk")]
    [ProvideLanguageService(Guids.StarkLanguageServiceIdString, "stark", languageResourceID: 101, RequestStockColors = true, ShowDropDownOptions = true)]

    // C# option pages tree:
    //   CSharp
    //     General (from editor)
    //     Scroll Bars (from editor)
    //     Tabs (from editor)
    //     Advanced
    //     Code Style (category)
    //       General
    //       Formatting (category)
    //         General
    //         Indentation
    //         New Lines
    //         Spacing
    //         Wrapping
    //       Naming
    //     IntelliSense

    [ProvideLanguageEditorOptionPage(typeof(Options.AdvancedOptionPage), "stark", null, "Advanced", pageNameResourceId: "#102", keywordListResourceId: 306)]
    [ProvideLanguageEditorToolsOptionCategory("stark", "Code Style", "#114")]
    [ProvideLanguageEditorOptionPage(typeof(Options.Formatting.CodeStylePage), "stark", @"Code Style", "General", pageNameResourceId: "#108", keywordListResourceId: 313)]
    [ProvideLanguageEditorToolsOptionCategory("stark", @"Code Style\Formatting", "#107")]
    [ProvideLanguageEditorOptionPage(typeof(Options.Formatting.FormattingOptionPage), "stark", @"Code Style\Formatting", "General", pageNameResourceId: "#108", keywordListResourceId: 307)]
    [ProvideLanguageEditorOptionPage(typeof(Options.Formatting.FormattingIndentationOptionPage), "stark", @"Code Style\Formatting", "Indentation", pageNameResourceId: "#109", keywordListResourceId: 308)]
    [ProvideLanguageEditorOptionPage(typeof(Options.Formatting.FormattingWrappingPage), "stark", @"Code Style\Formatting", "Wrapping", pageNameResourceId: "#110", keywordListResourceId: 311)]
    [ProvideLanguageEditorOptionPage(typeof(Options.Formatting.FormattingNewLinesPage), "stark", @"Code Style\Formatting", "NewLines", pageNameResourceId: "#111", keywordListResourceId: 309)]
    [ProvideLanguageEditorOptionPage(typeof(Options.Formatting.FormattingSpacingPage), "stark", @"Code Style\Formatting", "Spacing", pageNameResourceId: "#112", keywordListResourceId: 310)]
    [ProvideLanguageEditorOptionPage(typeof(Options.NamingStylesOptionPage), "stark", @"Code Style", "Naming", pageNameResourceId: "#115", keywordListResourceId: 314)]
    [ProvideLanguageEditorOptionPage(typeof(Options.IntelliSenseOptionPage), "stark", null, "IntelliSense", pageNameResourceId: "#103", keywordListResourceId: 312)]

    [ProvideAutomationProperties("TextEditor", "stark", Guids.TextManagerPackageString, profileNodeLabelId: 101, profileNodeDescriptionId: 106, resourcePackageGuid: Guids.StarkPackageIdString)]
    [ProvideAutomationProperties("TextEditor", "stark-Specific", packageGuid: Guids.StarkPackageIdString, profileNodeLabelId: 104, profileNodeDescriptionId: 105)]
    [ProvideService(typeof(StarkLanguageService), ServiceName = "Stark Language Service", IsAsyncQueryable = true)]
    [ProvideService(typeof(IStarkTempPECompilerService), ServiceName = "Stark TempPE Compiler Service", IsAsyncQueryable = true)]
    internal class StarkPackage : AbstractPackage<StarkPackage, StarkLanguageService>, IVsUserSettingsQuery
    {
        private ObjectBrowserLibraryManager _libraryManager;
        private uint _libraryManagerCookie;

        public StarkPackage()
        {
        }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            try
            {
                await base.InitializeAsync(cancellationToken, progress).ConfigureAwait(true);

                await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

                this.RegisterService<IStarkTempPECompilerService>(async ct =>
                {
                    await JoinableTaskFactory.SwitchToMainThreadAsync(ct);
                    return new TempPECompilerService(this.Workspace.Services.GetService<IMetadataService>());
                });

                await RegisterObjectBrowserLibraryManagerAsync(cancellationToken).ConfigureAwait(true);
            }
            catch (Exception e) when (FatalError.ReportUnlessCanceled(e))
            {
            }
        }

        protected override VisualStudioWorkspaceImpl CreateWorkspace()
        {
            return this.ComponentModel.GetService<VisualStudioWorkspaceImpl>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                JoinableTaskFactory.Run(() => UnregisterObjectBrowserLibraryManagerAsync(CancellationToken.None));
            }

            base.Dispose(disposing);
        }

        private async Task RegisterObjectBrowserLibraryManagerAsync(CancellationToken cancellationToken)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            if (await GetServiceAsync(typeof(SVsObjectManager)).ConfigureAwait(true) is IVsObjectManager2 objectManager)
            {
                _libraryManager = new ObjectBrowserLibraryManager(this, ComponentModel, Workspace);

                if (ErrorHandler.Failed(objectManager.RegisterSimpleLibrary(_libraryManager, out _libraryManagerCookie)))
                {
                    _libraryManagerCookie = 0;
                }
            }
        }

        private async Task UnregisterObjectBrowserLibraryManagerAsync(CancellationToken cancellationToken)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            if (_libraryManagerCookie != 0)
            {
                if (await GetServiceAsync(typeof(SVsObjectManager)).ConfigureAwait(true) is IVsObjectManager2 objectManager)
                {
                    objectManager.UnregisterLibrary(_libraryManagerCookie);
                    _libraryManagerCookie = 0;
                }

                _libraryManager.Dispose();
                _libraryManager = null;
            }
        }

        int IVsUserSettingsQuery.NeedExport(string pageID, out int needExport)
        {
            // We need to override MPF's definition of NeedExport since it doesn't know about our automation object
            needExport = (pageID == "TextEditor.CSharp-Specific") ? 1 : 0;

            return VSConstants.S_OK;
        }

        protected override object GetAutomationObject(string name)
        {
            if (name == "CSharp-Specific")
            {
                var workspace = this.ComponentModel.GetService<VisualStudioWorkspace>();
                return new Options.AutomationObject(workspace);
            }

            return base.GetAutomationObject(name);
        }

        protected override IEnumerable<IVsEditorFactory> CreateEditorFactories()
        {
            var editorFactory = new CSharpEditorFactory(this.ComponentModel);
            var codePageEditorFactory = new CSharpCodePageEditorFactory(editorFactory);

            return new IVsEditorFactory[] { editorFactory, codePageEditorFactory };
        }

        protected override StarkLanguageService CreateLanguageService()
        {
            return new StarkLanguageService(this);
        }

        protected override void RegisterMiscellaneousFilesWorkspaceInformation(MiscellaneousFilesWorkspace miscellaneousFilesWorkspace)
        {
            miscellaneousFilesWorkspace.RegisterLanguage(
                Guids.StarkLanguageServiceId,
                LanguageNames.Stark,
                ".csx");
        }

        protected override string RoslynLanguageName
        {
            get
            {
                return LanguageNames.Stark;
            }
        }
    }
}
