// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using StarkPlatform.CodeAnalysis.Options;
using StarkPlatform.CodeAnalysis.Options.Providers;

namespace StarkPlatform.CodeAnalysis.Editor.Options
{
    internal partial class ExtensionManagerOptions
    {
        public static readonly Option<bool> DisableCrashingExtensions = new Option<bool>(nameof(ExtensionManagerOptions), nameof(DisableCrashingExtensions), defaultValue: true);
    }

    [ExportOptionProvider, Shared]
    internal class ExtensionManagerOptionsProvider : IOptionProvider
    {
        public ImmutableArray<IOption> Options { get; } = ImmutableArray.Create<IOption>(
            ExtensionManagerOptions.DisableCrashingExtensions);
    }
}
