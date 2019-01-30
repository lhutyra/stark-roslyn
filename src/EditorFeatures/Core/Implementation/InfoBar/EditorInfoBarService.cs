// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Composition;
using StarkPlatform.CodeAnalysis.Extensions;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.CodeAnalysis.Internal.Log;

namespace StarkPlatform.CodeAnalysis.Editor.Implementation.Workspaces
{
    [ExportWorkspaceService(typeof(IInfoBarService)), Shared]
    internal class EditorInfoBarService : IInfoBarService
    {
        public void ShowInfoBarInActiveView(string message, params InfoBarUI[] items)
            => ShowInfoBarInGlobalView(message, items);

        public void ShowInfoBarInGlobalView(string message, params InfoBarUI[] items)
            => Logger.Log(FunctionId.Extension_InfoBar, message);
    }
}
