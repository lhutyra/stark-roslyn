﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis.Text;
using Cci = StarkPlatform.Cci;

namespace StarkPlatform.CodeAnalysis.CodeGen
{
    /// <summary>
    /// An expression that represents a (name, value) pair and that is typically used in method calls, custom attributes and object initializers.
    /// </summary>
    internal sealed class MetadataNamedArgument : Cci.IMetadataNamedArgument
    {
        private readonly ISymbol _entity;
        private readonly Cci.ITypeReference _type;
        private readonly Cci.IMetadataExpression _value;

        public MetadataNamedArgument(ISymbol entity, Cci.ITypeReference type, Cci.IMetadataExpression value)
        {
            // entity must be one of INamedEntity or IFieldDefinition or IPropertyDefinition
            _entity = entity;
            _type = type;
            _value = value;
        }

        /// <summary>
        /// The name of the parameter or property or field that corresponds to the argument.
        /// </summary>
        string Cci.IMetadataNamedArgument.ArgumentName => _entity.Name;

        /// <summary>
        /// The value of the argument.
        /// </summary>
        Cci.IMetadataExpression Cci.IMetadataNamedArgument.ArgumentValue => _value;

        /// <summary>
        /// True if the named argument provides the value of a field.
        /// </summary>
        bool Cci.IMetadataNamedArgument.IsField => _entity is Cci.IFieldDefinition;

        void Cci.IMetadataExpression.Dispatch(Cci.MetadataVisitor visitor)
        {
            visitor.Visit(this);
        }

        Cci.ITypeReference Cci.IMetadataExpression.Type => _type;
    }
}
