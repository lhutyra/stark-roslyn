using System;
using StarkPlatform.CodeAnalysis.Stark.Syntax;

namespace StarkPlatform.CodeAnalysis.Stark
{
    public partial class SyntaxFactory
    {
        public static ClassDeclarationSyntax ClassDeclaration(SyntaxList<AttributeSyntax> attributeLists, SyntaxTokenList modifiers, SyntaxToken identifier, TypeParameterListSyntax typeParameterList, ExtendListSyntax extendList, ImplementListSyntax implementList,
            SyntaxList<TypeParameterConstraintClauseSyntax> constraintClauses, SyntaxList<MemberDeclarationSyntax> members)
        {
            return SyntaxFactory.ClassDeclaration(attributeLists,
                modifiers,
                identifier,
                typeParameterList,
                extendList,
                implementList,
                constraintClauses,
                members,
                SyntaxFactory.EndOfLineToken());
        }

        public static StructDeclarationSyntax StructDeclaration(SyntaxList<AttributeSyntax> attributeLists, SyntaxTokenList modifiers, SyntaxToken identifier, TypeParameterListSyntax typeParameterList, ImplementListSyntax implementList,
            SyntaxList<TypeParameterConstraintClauseSyntax> constraintClauses, SyntaxList<MemberDeclarationSyntax> members)
        {
            return SyntaxFactory.StructDeclaration(attributeLists,
                modifiers,
                identifier,                
                typeParameterList,
                null,
                implementList,
                constraintClauses,
                members,
                SyntaxFactory.EndOfLineToken());
        }

        public static InterfaceDeclarationSyntax InterfaceDeclaration(SyntaxList<AttributeSyntax> attributeLists, SyntaxTokenList modifiers, SyntaxToken identifier, TypeParameterListSyntax typeParameterList,
            ExtendListSyntax extendList, SyntaxList<TypeParameterConstraintClauseSyntax> constraintClauses, SyntaxList<MemberDeclarationSyntax> members)
        {
            return SyntaxFactory.InterfaceDeclaration(attributeLists,
                modifiers,
                identifier,
                typeParameterList,
                extendList,
                null,
                constraintClauses,
                members,
                SyntaxFactory.EndOfLineToken());
        }

        public static BreakStatementSyntax BreakStatement()
        {
            return SyntaxFactory.BreakStatement(SyntaxFactory.Token(SyntaxKind.BreakKeyword), SyntaxFactory.EndOfLineToken());
        }

        public static EnumDeclarationSyntax EnumDeclaration(SyntaxList<AttributeSyntax> attributeLists, SyntaxTokenList modifiers, SyntaxToken identifier, ExtendListSyntax extendList,
            SeparatedSyntaxList<EnumMemberDeclarationSyntax> members)
        {
            return SyntaxFactory.EnumDeclaration(attributeLists,
                modifiers,
                identifier,
                extendList,
                null,
                members,
                SyntaxFactory.EndOfLineToken());
        }

        public static DelegateDeclarationSyntax DelegateDeclaration(SyntaxList<AttributeSyntax> attributeLists, SyntaxTokenList modifiers, TypeSyntax returnType, SyntaxToken identifier,
            TypeParameterListSyntax typeParameterList, ParameterListSyntax parameterList, SyntaxList<TypeParameterConstraintClauseSyntax> constraintClauses)
        {
            return SyntaxFactory.DelegateDeclaration(
                default,
                modifiers,
                returnType != null ? (TypeSyntax)returnType : SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                identifier,
                typeParameterList,
                parameterList,
                constraintClauses,
                SyntaxFactory.EndOfLineToken());
        }

        public static AttributeSyntax Attribute(NameSyntax name, AttributeArgumentListSyntax argumentList)
        {
            return SyntaxFactory.Attribute(default, name, argumentList);
        }


        public static ReturnStatementSyntax ReturnStatement(ExpressionSyntax expression)
        {
            return SyntaxFactory.ReturnStatement(expression, SyntaxFactory.EndOfLineToken());
        }

        public static ThrowStatementSyntax ThrowStatement(ExpressionSyntax expression)
        {
            return SyntaxFactory.ThrowStatement(expression, SyntaxFactory.EndOfLineToken());
        }


        public static IfStatementSyntax IfStatement(ExpressionSyntax condition, BlockSyntax statement)
        {
            return SyntaxFactory.IfStatement(condition, statement, null);
        }


        public static ExpressionStatementSyntax ExpressionStatement(ExpressionSyntax expression)
        {
            return SyntaxFactory.ExpressionStatement(expression, SyntaxFactory.EndOfLineToken());
        }

        public static LocalDeclarationStatementSyntax LocalDeclarationStatement(VariableDeclarationSyntax declaration)
        {
            return SyntaxFactory.LocalDeclarationStatement(declaration, SyntaxFactory.EndOfLineToken());
        }

        public static FieldDeclarationSyntax FieldDeclaration(SyntaxList<AttributeSyntax> attributeLists, SyntaxTokenList modifiers, VariableDeclarationSyntax declaration)
        {
            return SyntaxFactory.FieldDeclaration(attributeLists, modifiers, declaration, EndOfLineToken());
        }

        public static ImportDirectiveSyntax ImportDirective(NameSyntax name)
        {
            return SyntaxFactory.ImportDirective(name, SyntaxFactory.EndOfLineToken());
        }


        public static IndexerDeclarationSyntax IndexerDeclaration(SyntaxList<AttributeSyntax> attributeLists, SyntaxTokenList modifiers, ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier, BracketedParameterListSyntax parameterList, TypeSyntax type, AccessorListSyntax accessorList, ArrowExpressionClauseSyntax expressionBody = null)
        {
            return SyntaxFactory.IndexerDeclaration(attributeLists, modifiers, SyntaxFactory.Token(SyntaxKind.FuncKeyword), SyntaxFactory.Token(SyntaxKind.OperatorKeyword), explicitInterfaceSpecifier, parameterList,
                SyntaxFactory.Token(SyntaxKind.MinusGreaterThanToken), type, accessorList, expressionBody, expressionBody != null ? SyntaxFactory.EndOfLineToken() : default);
        }

        public static PropertyDeclarationSyntax PropertyDeclaration(
            SyntaxList<AttributeSyntax> attributeLists,
            SyntaxTokenList modifiers,
            ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier,
            SyntaxToken identifier,
            TypeSyntax type,
            AccessorListSyntax accessorList = null,
            ArrowExpressionClauseSyntax expressionBody = null,
            EqualsValueClauseSyntax initializer = null
            )
        {
            return SyntaxFactory.PropertyDeclaration(
                attributeLists,
                modifiers,
                SyntaxFactory.Token(SyntaxKind.FuncKeyword),
                explicitInterfaceSpecifier,
                identifier,
                SyntaxFactory.Token(SyntaxKind.MinusGreaterThanToken),
                type,
                accessorList,
                expressionBody,
                initializer,
                expressionBody != null || initializer != null ? SyntaxFactory.EndOfLineToken() : default);
        }

        public static ExternAliasDirectiveSyntax ExternAliasDirective(string identifier)
        {
            return SyntaxFactory.ExternAliasDirective(identifier, SyntaxFactory.EndOfLineToken());
        }

        public static ExternAliasDirectiveSyntax ExternAliasDirective(SyntaxToken identifier)
        {
            return SyntaxFactory.ExternAliasDirective(SyntaxFactory.Token(SyntaxKind.ExternKeyword), SyntaxFactory.Token(SyntaxKind.AliasKeyword), identifier, EndOfLineToken());
        }

        /// <summary>Creates a new ReturnStatementSyntax instance.</summary>
        public static ReturnStatementSyntax ReturnStatement()
        {
            return SyntaxFactory.ReturnStatement(EndOfLineToken());
        }

        public static ContinueStatementSyntax ContinueStatement()
        {
            return SyntaxFactory.ContinueStatement(EndOfLineToken());
        }

    }
}
