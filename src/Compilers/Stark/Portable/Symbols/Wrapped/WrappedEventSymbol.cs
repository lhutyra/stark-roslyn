﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace StarkPlatform.CodeAnalysis.Stark.Symbols
{
    /// <summary>
    /// Represents an event that is based on another event.
    /// When inheriting from this class, one shouldn't assume that 
    /// the default behavior it has is appropriate for every case.
    /// That behavior should be carefully reviewed and derived type
    /// should override behavior as appropriate.
    /// </summary>
    internal abstract class WrappedEventSymbol : EventSymbol
    {
        /// <summary>
        /// The underlying EventSymbol.
        /// </summary>
        protected readonly EventSymbol _underlyingEvent;

        public WrappedEventSymbol(EventSymbol underlyingEvent)
        {
            Debug.Assert((object)underlyingEvent != null);
            _underlyingEvent = underlyingEvent;
        }

        public EventSymbol UnderlyingEvent
        {
            get
            {
                return _underlyingEvent;
            }
        }

        public override bool IsImplicitlyDeclared
        {
            get
            {
                return _underlyingEvent.IsImplicitlyDeclared;
            }
        }

        internal override bool HasSpecialName
        {
            get
            {
                return _underlyingEvent.HasSpecialName;
            }
        }

        public override string Name
        {
            get
            {
                return _underlyingEvent.Name;
            }
        }

        public override string GetDocumentationCommentXml(CultureInfo preferredCulture = null, bool expandIncludes = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _underlyingEvent.GetDocumentationCommentXml(preferredCulture, expandIncludes, cancellationToken);
        }

        public override ImmutableArray<Location> Locations
        {
            get
            {
                return _underlyingEvent.Locations;
            }
        }

        public override ImmutableArray<SyntaxReference> DeclaringSyntaxReferences
        {
            get
            {
                return _underlyingEvent.DeclaringSyntaxReferences;
            }
        }

        public override Accessibility DeclaredAccessibility
        {
            get
            {
                return _underlyingEvent.DeclaredAccessibility;
            }
        }

        public override bool IsStatic
        {
            get
            {
                return _underlyingEvent.IsStatic;
            }
        }

        public override bool IsVirtual
        {
            get
            {
                return _underlyingEvent.IsVirtual;
            }
        }

        public override bool IsOverride
        {
            get
            {
                return _underlyingEvent.IsOverride;
            }
        }

        public override bool IsAbstract
        {
            get
            {
                return _underlyingEvent.IsAbstract;
            }
        }

        public override bool IsSealed
        {
            get
            {
                return _underlyingEvent.IsSealed;
            }
        }

        public override bool IsExtern
        {
            get
            {
                return _underlyingEvent.IsExtern;
            }
        }

        internal override ObsoleteAttributeData ObsoleteAttributeData
        {
            get
            {
                return _underlyingEvent.ObsoleteAttributeData;
            }
        }

        public override bool IsWindowsRuntimeEvent
        {
            get
            {
                return _underlyingEvent.IsWindowsRuntimeEvent;
            }
        }

        internal override bool HasRuntimeSpecialName
        {
            get
            {
                return _underlyingEvent.HasRuntimeSpecialName;
            }
        }
    }
}
