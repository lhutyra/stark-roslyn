// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Composition;
using StarkPlatform.CodeAnalysis.Editor.Implementation.RenameTracking;
using StarkPlatform.CodeAnalysis.Host.Mef;

namespace StarkPlatform.CodeAnalysis.Editor.CSharp.RenameTracking
{
    [ExportLanguageService(typeof(IRenameTrackingLanguageHeuristicsService), LanguageNames.Stark), Shared]
    internal sealed class CSharpRenameTrackingLanguageHeuristicsService : IRenameTrackingLanguageHeuristicsService
    {
        public bool IsIdentifierValidForRenameTracking(string name)
        {
            return name != "var" && name != "dynamic";
        }
    }
}
