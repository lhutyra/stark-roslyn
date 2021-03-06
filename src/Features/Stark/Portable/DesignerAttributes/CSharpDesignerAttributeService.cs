﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Composition;
using System.Linq;
using StarkPlatform.CodeAnalysis.Stark.Syntax;
using StarkPlatform.CodeAnalysis.DesignerAttributes;
using StarkPlatform.CodeAnalysis.Host;
using StarkPlatform.CodeAnalysis.Host.Mef;
using Roslyn.Utilities;

namespace StarkPlatform.CodeAnalysis.Stark.DesignerAttributes
{
    [ExportLanguageServiceFactory(typeof(IDesignerAttributeService), LanguageNames.Stark), Shared]
    internal class CSharpDesignerAttributeServiceFactory : ILanguageServiceFactory
    {
        public ILanguageService CreateLanguageService(HostLanguageServices languageServices)
            => new CSharpDesignerAttributeService(languageServices.WorkspaceServices.Workspace);
    }

    internal class CSharpDesignerAttributeService : AbstractDesignerAttributeService
    {
        public CSharpDesignerAttributeService(Workspace workspace) : base(workspace)
        {
        }

        protected override IEnumerable<SyntaxNode> GetAllTopLevelTypeDefined(SyntaxNode node)
        {
            var compilationUnit = node as CompilationUnitSyntax;
            if (compilationUnit == null)
            {
                return SpecializedCollections.EmptyEnumerable<SyntaxNode>();
            }

            return compilationUnit.Members.SelectMany(GetAllTopLevelTypeDefined);
        }

        private IEnumerable<SyntaxNode> GetAllTopLevelTypeDefined(MemberDeclarationSyntax member)
        {
            switch (member)
            {
                case NamespaceDeclarationSyntax namespaceMember:
                    return namespaceMember.Members.SelectMany(GetAllTopLevelTypeDefined);
                case ClassDeclarationSyntax type:
                    return SpecializedCollections.SingletonEnumerable<SyntaxNode>(type);
            }

            return SpecializedCollections.EmptyEnumerable<SyntaxNode>();
        }

        protected override bool ProcessOnlyFirstTypeDefined()
        {
            return true;
        }

        protected override bool HasAttributesOrBaseTypeOrIsPartial(SyntaxNode typeNode)
        {
            if (typeNode is ClassDeclarationSyntax classNode)
            {
                return classNode.AttributeLists.Count > 0 ||
                    classNode.ImplementList != null ||
                    classNode.Modifiers.Any(SyntaxKind.PartialKeyword);
            }

            return false;
        }
    }
}
