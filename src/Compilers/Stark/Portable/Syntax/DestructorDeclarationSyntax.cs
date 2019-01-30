// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis.Stark.Symbols;
using StarkPlatform.CodeAnalysis.Stark.Syntax;
using StarkPlatform.CodeAnalysis.Text;

namespace StarkPlatform.CodeAnalysis.Stark.Syntax
{
    public partial class DestructorDeclarationSyntax
    {
        public DestructorDeclarationSyntax Update(
            SyntaxList<AttributeSyntax> attributeLists,
            SyntaxTokenList modifiers,
            SyntaxToken tildeToken,
            SyntaxToken identifier,
            ParameterListSyntax parameterList,
            BlockSyntax body,
            SyntaxToken semicolonToken)
            => Update(
                attributeLists,
                modifiers,
                tildeToken,
                identifier,
                parameterList,
                body,
                default(ArrowExpressionClauseSyntax),
                semicolonToken);
    }
}

namespace StarkPlatform.CodeAnalysis.Stark
{
    public partial class SyntaxFactory
    {
        public static DestructorDeclarationSyntax DestructorDeclaration(
            SyntaxList<AttributeSyntax> attributeLists,
            SyntaxTokenList modifiers,
            SyntaxToken identifier,
            ParameterListSyntax parameterList,
            BlockSyntax body)
            => DestructorDeclaration(
                attributeLists,
                modifiers,
                SyntaxFactory.Token(SyntaxKind.TildeToken),
                identifier,
                parameterList,
                body,
                default(ArrowExpressionClauseSyntax),
                default(SyntaxToken));

        public static DestructorDeclarationSyntax DestructorDeclaration(
            SyntaxList<AttributeSyntax> attributeLists,
            SyntaxTokenList modifiers,
            SyntaxToken tildeToken,
            SyntaxToken identifier,
            ParameterListSyntax parameterList,
            BlockSyntax body,
            SyntaxToken semicolonToken)
            => DestructorDeclaration(
                attributeLists,
                modifiers,
                tildeToken,
                identifier,
                parameterList,
                body,
                default(ArrowExpressionClauseSyntax),
                semicolonToken);

        public static DestructorDeclarationSyntax DestructorDeclaration(
            SyntaxList<AttributeSyntax> attributeLists,
            SyntaxTokenList modifiers,
            SyntaxToken identifier,
            ParameterListSyntax parameterList,
            ArrowExpressionClauseSyntax expressionBody)
            => DestructorDeclaration(
                attributeLists,
                modifiers,
                SyntaxFactory.Token(SyntaxKind.TildeToken),
                identifier,
                parameterList,
                default(BlockSyntax),
                expressionBody,
                default(SyntaxToken));

        public static DestructorDeclarationSyntax DestructorDeclaration(
            SyntaxList<AttributeSyntax> attributeLists,
            SyntaxTokenList modifiers,
            SyntaxToken tildeToken,
            SyntaxToken identifier,
            ParameterListSyntax parameterList,
            ArrowExpressionClauseSyntax expressionBody,
            SyntaxToken semicolonToken)
            => DestructorDeclaration(
                attributeLists,
                modifiers,
                tildeToken,
                identifier,
                parameterList,
                default(BlockSyntax),
                expressionBody,
                semicolonToken);

    }
}
