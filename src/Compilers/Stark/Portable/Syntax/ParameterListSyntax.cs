﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis.Stark.Symbols;
using StarkPlatform.CodeAnalysis.Stark.Syntax;
using StarkPlatform.CodeAnalysis.Text;

namespace StarkPlatform.CodeAnalysis.Stark.Syntax
{
    public partial class ParameterListSyntax
    {
        internal int ParameterCount
        {
            get
            {
                int count = 0;
                foreach (ParameterSyntax parameter in this.Parameters)
                {
                    // __arglist does not affect the parameter count.
                    if (!parameter.IsArgList)
                    {
                        count++;
                    }
                }
                return count;
            }
        }
    }
}
