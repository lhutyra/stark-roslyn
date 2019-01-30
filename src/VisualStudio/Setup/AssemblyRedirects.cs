// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using Microsoft.VisualStudio.Shell;

[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\SQLitePCLRaw.batteries_green.DLL")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\SQLitePCLRaw.batteries_v2.DLL")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\SQLitePCLRaw.core.DLL")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\SQLitePCLRaw.provider.e_sqlite3.DLL")]

[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\System.Composition.Convention.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\System.Composition.Hosting.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\System.Composition.TypedParts.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\Humanizer.dll")]

[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\ICSharpCode.Decompiler.dll")]

[assembly: ProvideBindingRedirection(
    AssemblyName = "Microsoft.VisualStudio.CallHierarchy.Package.Definitions",
    GenerateCodeBase = false,
    PublicKeyToken = "31BF3856AD364E35",
    OldVersionLowerBound = "14.0.0.0",
    OldVersionUpperBound = "14.9.9.9",
    NewVersion = "15.0.0.0")]
