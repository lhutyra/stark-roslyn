﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using StarkPlatform.CodeAnalysis.DocumentationComments;

namespace StarkPlatform.CodeAnalysis.MetadataAsSource
{
    internal partial class AbstractMetadataAsSourceService
    {
        private class WrappedFieldSymbol : AbstractWrappedSymbol, IFieldSymbol
        {
            private readonly IFieldSymbol _symbol;

            public WrappedFieldSymbol(IFieldSymbol fieldSymbol, IDocumentationCommentFormattingService docCommentFormattingService)
                : base(fieldSymbol, canImplementImplicitly: false, docCommentFormattingService: docCommentFormattingService)
            {
                _symbol = fieldSymbol;
            }

            public new IFieldSymbol OriginalDefinition => _symbol.OriginalDefinition;

            public IFieldSymbol CorrespondingTupleField => null;

            public ISymbol AssociatedSymbol => _symbol.AssociatedSymbol;

            public object ConstantValue => _symbol.ConstantValue;

            public ImmutableArray<CustomModifier> CustomModifiers => _symbol.CustomModifiers;

            public bool HasConstantValue => _symbol.HasConstantValue;

            public bool IsConst => _symbol.IsConst;

            public bool IsLet => _symbol.IsLet;

            public bool IsVolatile => _symbol.IsVolatile;

            public bool IsFixedSizeBuffer => _symbol.IsFixedSizeBuffer;

            public ITypeSymbol Type => _symbol.Type;
        }
    }
}
