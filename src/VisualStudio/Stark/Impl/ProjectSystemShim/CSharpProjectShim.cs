// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Editor.Shared.Utilities;
using StarkPlatform.CodeAnalysis.Host;
using Microsoft.VisualStudio.ComponentModelHost;
using StarkPlatform.VisualStudio.LanguageServices.CSharp.ProjectSystemShim.Interop;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.CodeModel;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.ProjectSystem;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.ProjectSystem.Legacy;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.TaskList;
using Microsoft.VisualStudio.Shell.Interop;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.ProjectSystemShim
{
    /// <summary>
    /// The representation of a project to both the project factory and workspace API.
    /// </summary>
    /// <remarks>
    /// Due to the number of interfaces this object must implement, all interface implementations
    /// are in a separate files. Methods that are shared across multiple interfaces (which are
    /// effectively methods that just QI from one interface to another), are implemented here.
    /// </remarks>
    internal sealed partial class CSharpProjectShim : AbstractLegacyProject, ICodeModelInstanceFactory
    {
        /// <summary>
        /// This member is used to store a raw array of warning numbers, which is needed to properly implement
        /// ICSCompilerConfig.GetWarnNumbers. Read the implementation of that function for more details.
        /// </summary>
        private readonly IntPtr _warningNumberArrayPointer;

        private ICSharpProjectRoot _projectRoot;

        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Fetches the options processor for this C# project. Equivalent to the underlying member, but fixed to the derived type.
        /// </summary>
        private new OptionsProcessor VisualStudioProjectOptionsProcessor
        {
            get => (OptionsProcessor)base.VisualStudioProjectOptionsProcessor;
            set => base.VisualStudioProjectOptionsProcessor = value;
        }

        public CSharpProjectShim(
            ICSharpProjectRoot projectRoot,
            string projectSystemName,
            IVsHierarchy hierarchy,
            IServiceProvider serviceProvider,
            IThreadingContext threadingContext,
            HostDiagnosticUpdateSource hostDiagnosticUpdateSourceOpt,
            ICommandLineParserService commandLineParserServiceOpt)
            : base(projectSystemName,
                   hierarchy,
                   LanguageNames.Stark,
                   serviceProvider,
                   threadingContext,
                   externalErrorReportingPrefix: "CS",
                   hostDiagnosticUpdateSourceOpt: hostDiagnosticUpdateSourceOpt,
                   commandLineParserServiceOpt: commandLineParserServiceOpt)
        {
            _projectRoot = projectRoot;
            _serviceProvider = serviceProvider;
            _warningNumberArrayPointer = Marshal.AllocHGlobal(0);

            var componentModel = (IComponentModel)serviceProvider.GetService(typeof(SComponentModel));

            this.ProjectCodeModel = componentModel.GetService<IProjectCodeModelFactory>().CreateProjectCodeModel(VisualStudioProject.Id, this);
            this.VisualStudioProjectOptionsProcessor = new OptionsProcessor(this.VisualStudioProject, Workspace.Services);

            // Ensure the default options are set up
            ResetAllOptions();
        }

        public override void Disconnect()
        {
            _projectRoot = null;

            base.Disconnect();
        }

        ~CSharpProjectShim()
        {
            // Free the unmanaged memory we allocated in the constructor
            Marshal.FreeHGlobal(_warningNumberArrayPointer);

            // Free any entry point strings.
            if (_startupClasses != null)
            {
                foreach (var @class in _startupClasses)
                {
                    Marshal.FreeHGlobal(@class);
                }
            }
        }

        EnvDTE.FileCodeModel ICodeModelInstanceFactory.TryCreateFileCodeModelThroughProjectSystem(string filePath)
        {
            if (_projectRoot.CanCreateFileCodeModel(filePath))
            {
                var iid = VSConstants.IID_IUnknown;
                return _projectRoot.CreateFileCodeModel(filePath, ref iid) as EnvDTE.FileCodeModel;
            }
            else
            {
                return null;
            }
        }
    }
}
