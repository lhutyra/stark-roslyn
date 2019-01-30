// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Stark;
using StarkPlatform.CodeAnalysis.Stark.Extensions;
using StarkPlatform.CodeAnalysis.Stark.Symbols;
using StarkPlatform.CodeAnalysis.Stark.Syntax;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.CodeModel
{
    internal static class ParameterFlagsExtensions
    {
        public static ParameterFlags GetParameterFlags(this ParameterSyntax parameter)
        {
            ParameterFlags result = 0;

            foreach (var modifier in parameter.Modifiers)
            {
                switch (modifier.Kind())
                {
                    case SyntaxKind.RefKeyword:
                        result |= ParameterFlags.Ref;
                        break;
                    case SyntaxKind.OutKeyword:
                        result |= ParameterFlags.Out;
                        break;
                    case SyntaxKind.ParamsKeyword:
                        result |= ParameterFlags.Params;
                        break;
                }
            }

            return result;
        }
    }
}
