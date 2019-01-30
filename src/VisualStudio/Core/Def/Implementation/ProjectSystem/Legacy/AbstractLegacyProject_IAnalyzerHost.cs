﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.IO;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.ProjectSystem.Interop;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.ProjectSystem.Legacy
{
    internal abstract partial class AbstractLegacyProject : IAnalyzerHost
    {
        void IAnalyzerHost.AddAnalyzerReference(string analyzerAssemblyFullPath)
        {
            VisualStudioProject.AddAnalyzerReference(analyzerAssemblyFullPath);
        }

        void IAnalyzerHost.RemoveAnalyzerReference(string analyzerAssemblyFullPath)
        {
            VisualStudioProject.RemoveAnalyzerReference(analyzerAssemblyFullPath);
        }

        void IAnalyzerHost.SetRuleSetFile(string ruleSetFileFullPath)
        {
            // Sometimes the project system hands us paths with extra backslashes
            // and passing that to other parts of the shell causes issues
            // http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1087250
            if (!string.IsNullOrEmpty(ruleSetFileFullPath))
            {
                ruleSetFileFullPath = Path.GetFullPath(ruleSetFileFullPath);
            }
            else
            {
                ruleSetFileFullPath = null;
            }

            VisualStudioProjectOptionsProcessor.ExplicitRuleSetFilePath = ruleSetFileFullPath;
        }

        void IAnalyzerHost.AddAdditionalFile(string additionalFilePath)
        {
            VisualStudioProject.AddAdditionalFile(additionalFilePath);
        }

        void IAnalyzerHost.RemoveAdditionalFile(string additionalFilePath)
        {
            VisualStudioProject.RemoveAdditionalFile(additionalFilePath);
        }
    }
}
