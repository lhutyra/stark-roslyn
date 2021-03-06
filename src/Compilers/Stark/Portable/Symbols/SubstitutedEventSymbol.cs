﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading;

namespace StarkPlatform.CodeAnalysis.Stark.Symbols
{
    internal sealed class SubstitutedEventSymbol : WrappedEventSymbol
    {
        private readonly SubstitutedNamedTypeSymbol _containingType;

        private TypeSymbolWithAnnotations.Builder _lazyType;

        internal SubstitutedEventSymbol(SubstitutedNamedTypeSymbol containingType, EventSymbol originalDefinition)
            : base(originalDefinition)
        {
            Debug.Assert(originalDefinition.IsDefinition);
            _containingType = containingType;
        }

        public override TypeSymbolWithAnnotations Type
        {
            get
            {
                if (_lazyType.IsNull)
                {
                    _lazyType.InterlockedInitialize(_containingType.TypeSubstitution.SubstituteTypeWithTupleUnification(OriginalDefinition.Type));
                }

                return _lazyType.ToType();
            }
        }

        public override Symbol ContainingSymbol
        {
            get
            {
                return _containingType;
            }
        }

        public override EventSymbol OriginalDefinition
        {
            get
            {
                return _underlyingEvent;
            }
        }

        public override ImmutableArray<CSharpAttributeData> GetAttributes()
        {
            return OriginalDefinition.GetAttributes();
        }

        public override MethodSymbol AddMethod
        {
            get
            {
                MethodSymbol originalAddMethod = OriginalDefinition.AddMethod;
                return (object)originalAddMethod == null ? null : originalAddMethod.AsMember(_containingType);
            }
        }

        public override MethodSymbol RemoveMethod
        {
            get
            {
                MethodSymbol originalRemoveMethod = OriginalDefinition.RemoveMethod;
                return (object)originalRemoveMethod == null ? null : originalRemoveMethod.AsMember(_containingType);
            }
        }

        internal override FieldSymbol AssociatedField
        {
            get
            {
                FieldSymbol originalAssociatedField = OriginalDefinition.AssociatedField;
                return (object)originalAssociatedField == null ? null : originalAssociatedField.AsMember(_containingType);
            }
        }

        internal override bool IsExplicitInterfaceImplementation
        {
            get { return OriginalDefinition.IsExplicitInterfaceImplementation; }
        }

        //we want to compute this lazily since it may be expensive for the underlying symbol
        private ImmutableArray<EventSymbol> _lazyExplicitInterfaceImplementations;

        private OverriddenOrHiddenMembersResult _lazyOverriddenOrHiddenMembers;

        public override ImmutableArray<EventSymbol> ExplicitInterfaceImplementations
        {
            get
            {
                if (_lazyExplicitInterfaceImplementations.IsDefault)
                {
                    ImmutableInterlocked.InterlockedCompareExchange(
                        ref _lazyExplicitInterfaceImplementations,
                        ExplicitInterfaceHelpers.SubstituteExplicitInterfaceImplementations(OriginalDefinition.ExplicitInterfaceImplementations, _containingType.TypeSubstitution),
                        default(ImmutableArray<EventSymbol>));
                }
                return _lazyExplicitInterfaceImplementations;
            }
        }

        internal override bool MustCallMethodsDirectly
        {
            get { return OriginalDefinition.MustCallMethodsDirectly; }
        }

        internal override OverriddenOrHiddenMembersResult OverriddenOrHiddenMembers
        {
            get
            {
                if (_lazyOverriddenOrHiddenMembers == null)
                {
                    Interlocked.CompareExchange(ref _lazyOverriddenOrHiddenMembers, this.MakeOverriddenOrHiddenMembers(), null);
                }
                return _lazyOverriddenOrHiddenMembers;
            }
        }

        public override bool IsWindowsRuntimeEvent
        {
            get
            {
                // A substituted event computes overriding and interface implementation separately
                // from the original definition, in case the type has changed.  However, is should
                // never be the case that providing type arguments changes a WinRT event to a 
                // non-WinRT event or vice versa, so we'll delegate to the original definition.
                return OriginalDefinition.IsWindowsRuntimeEvent;
            }
        }
    }
}
