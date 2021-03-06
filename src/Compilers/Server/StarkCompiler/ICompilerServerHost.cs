﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StarkPlatform.CodeAnalysis.CommandLine;

namespace StarkPlatform.CodeAnalysis.CompilerServer
{
    internal interface ICompilerServerHost
    {
        BuildResponse RunCompilation(RunRequest request, CancellationToken cancellationToken);
    }
}
