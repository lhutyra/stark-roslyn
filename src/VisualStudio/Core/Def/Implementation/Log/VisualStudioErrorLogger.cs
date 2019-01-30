// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Composition;
using StarkPlatform.CodeAnalysis.ErrorLogger;
using StarkPlatform.CodeAnalysis.ErrorReporting;
using StarkPlatform.CodeAnalysis.Host.Mef;
using Microsoft.VisualStudio.Shell;
using static StarkPlatform.CodeAnalysis.RoslynAssemblyHelper;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.Log
{
    [ExportWorkspaceService(typeof(IErrorLoggerService), ServiceLayer.Host), Export(typeof(IErrorLoggerService)), Shared]
    internal class VisualStudioErrorLogger : IErrorLoggerService
    {
        public void LogException(object source, Exception exception)
        {
            var name = source.GetType().Name;
            ActivityLog.LogError(name, ToLogFormat(exception));
        }

        private bool ShouldReportCrashDumps(object source) => HasRoslynPublicKey(source);

        private static string ToLogFormat(Exception exception)
        {
            return exception.Message + Environment.NewLine + exception.StackTrace;
        }
    }
}
