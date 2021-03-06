﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis.Stark.Symbols;
using StarkPlatform.CodeAnalysis.Stark.Syntax;
using StarkPlatform.CodeAnalysis.Text;

namespace StarkPlatform.CodeAnalysis.Stark
{
    internal abstract class LoopBinder : LocalScopeBinder
    {
        private readonly GeneratedLabelSymbol _breakLabel;
        private readonly GeneratedLabelSymbol _continueLabel;

        protected LoopBinder(Binder enclosing)
            : base(enclosing)
        {
            _breakLabel = new GeneratedLabelSymbol("break");
            _continueLabel = new GeneratedLabelSymbol("continue");
        }

        internal override GeneratedLabelSymbol BreakLabel
        {
            get
            {
                return _breakLabel;
            }
        }

        internal override GeneratedLabelSymbol ContinueLabel
        {
            get
            {
                return _continueLabel;
            }
        }
    }
}
