﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading;
using StarkPlatform.CodeAnalysis.Stark.Extensions;
using StarkPlatform.CodeAnalysis.Stark.Extensions.ContextQuery;
using StarkPlatform.CodeAnalysis.Stark.Syntax;
using StarkPlatform.CodeAnalysis.Shared.Extensions;

namespace StarkPlatform.CodeAnalysis.Stark.Completion.KeywordRecommenders
{
    internal class ByKeywordRecommender : AbstractSyntacticSingleKeywordRecommender
    {
        public ByKeywordRecommender()
            : base(SyntaxKind.ByKeyword)
        {
        }

        protected override bool IsValidContext(int position, CSharpSyntaxContext context, CancellationToken cancellationToken)
        {
            // cases:
            //   group e |
            //   group e b|

            var token = context.LeftToken;
            var group = token.GetAncestor<GroupClauseSyntax>();

            if (group == null)
            {
                return false;
            }

            var lastToken = group.GroupExpression.GetLastToken(includeSkipped: true);

            // group e |
            if (!token.IntersectsWith(position) &&
                token == lastToken)
            {
                return true;
            }

            // group e b|
            if (token.IntersectsWith(position) &&
                token.Kind() == SyntaxKind.IdentifierToken &&
                token.GetPreviousToken(includeSkipped: true) == lastToken)
            {
                return true;
            }

            return false;
        }
    }
}
