// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.CodeAnalysis.CSharp.Syntax
{
    public partial class ForEachVariableStatementSyntax
    {
        public ForEachVariableStatementSyntax Update(SyntaxToken forEachKeyword, ExpressionSyntax variable, SyntaxToken inKeyword, ExpressionSyntax expression, BlockSyntax statement)
        {
            return Update(awaitKeyword: default, forEachKeyword, variable, inKeyword, expression, statement);
        }
    }
}

namespace Microsoft.CodeAnalysis.CSharp
{
    public partial class SyntaxFactory
    {
        public static ForEachVariableStatementSyntax ForEachVariableStatement(SyntaxToken forEachKeyword, ExpressionSyntax variable, SyntaxToken inKeyword, ExpressionSyntax expression, BlockSyntax statement)
        {
            return ForEachVariableStatement(awaitKeyword: default, forEachKeyword, variable, inKeyword, expression, statement);
        }
    }
}
