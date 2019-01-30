﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Runtime.InteropServices;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.CodeModel.Collections;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.Interop;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.Utilities;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.CodeModel.InternalElements
{
    [ComVisible(true)]
    [ComDefaultInterface(typeof(EnvDTE80.CodeElement2))]
    public sealed class CodeImplementsStatement : AbstractCodeElement
    {
        internal static EnvDTE80.CodeElement2 Create(
            CodeModelState state,
            AbstractCodeMember parent,
            string namespaceName,
            int ordinal)
        {
            var element = new CodeImplementsStatement(state, parent, namespaceName, ordinal);
            var result = (EnvDTE80.CodeElement2)ComAggregate.CreateAggregatedObject(element);

            return result;
        }

        internal static EnvDTE80.CodeElement2 CreateUnknown(
            CodeModelState state,
            FileCodeModel fileCodeModel,
            int nodeKind,
            string name)
        {
            var element = new CodeImplementsStatement(state, fileCodeModel, nodeKind, name);
            return (EnvDTE80.CodeElement2)ComAggregate.CreateAggregatedObject(element);
        }

        private readonly ParentHandle<AbstractCodeMember> _parentHandle;
        private readonly string _namespaceName;
        private readonly int _ordinal;

        private CodeImplementsStatement(
            CodeModelState state,
            AbstractCodeMember parent,
            string namespaceName,
            int ordinal)
            : base(state, parent.FileCodeModel)
        {
            _parentHandle = new ParentHandle<AbstractCodeMember>(parent);
            _namespaceName = namespaceName;
            _ordinal = ordinal;
        }

        private CodeImplementsStatement(
            CodeModelState state,
            FileCodeModel fileCodeModel,
            int nodeKind,
            string name)
            : base(state, fileCodeModel, nodeKind)
        {
            _namespaceName = name;
        }

        internal override bool TryLookupNode(out SyntaxNode node)
        {
            node = null;

            var parentNode = _parentHandle.Value.LookupNode();
            if (parentNode == null)
            {
                return false;
            }

            if (!CodeModelService.TryGetImplementsNode(parentNode, _namespaceName, _ordinal, out var implementsNode))
            {
                return false;
            }

            node = implementsNode;
            return node != null;
        }

        public override EnvDTE.vsCMElement Kind
        {
            get { return EnvDTE.vsCMElement.vsCMElementImplementsStmt; }
        }

        public override object Parent
        {
            get { return _parentHandle.Value; }
        }

        public override EnvDTE.CodeElements Children
        {
            get { return EmptyCollection.Create(this.State, this); }
        }

        protected override void SetName(string value)
        {
            throw Exceptions.ThrowENotImpl();
        }

        public override void RenameSymbol(string newName)
        {
            throw Exceptions.ThrowENotImpl();
        }
    }
}
