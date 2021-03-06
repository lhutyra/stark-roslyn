﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis.Options;

namespace StarkPlatform.CodeAnalysis.CodeCleanup
{
    /// <summary>
    /// Indicates which, if any, Organize Usings features are enabled for code cleanup.
    /// </summary>
    internal sealed class OrganizeUsingsSet
    {
        public bool IsRemoveUnusedImportEnabled { get; }
        public bool IsSortImportsEnabled { get; }

        public OrganizeUsingsSet(OptionSet optionSet, string language)
        {
            IsRemoveUnusedImportEnabled = optionSet.GetOption(CodeCleanupOptions.RemoveUnusedImports, language);
            IsSortImportsEnabled = optionSet.GetOption(CodeCleanupOptions.SortImports, language);
        }

        public OrganizeUsingsSet(bool isRemoveUnusedImportEnabled, bool isSortImportsEnabled)
        {
            IsRemoveUnusedImportEnabled = isRemoveUnusedImportEnabled;
            IsSortImportsEnabled = isSortImportsEnabled;
        }
    }
}
