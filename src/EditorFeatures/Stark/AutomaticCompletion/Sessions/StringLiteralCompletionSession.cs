// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading;
using StarkPlatform.CodeAnalysis.Stark;
using StarkPlatform.CodeAnalysis.Editor.Implementation.AutomaticCompletion;
using StarkPlatform.CodeAnalysis.Editor.Implementation.AutomaticCompletion.Sessions;
using StarkPlatform.CodeAnalysis.LanguageServices;
using Microsoft.VisualStudio.Text.BraceCompletion;

namespace StarkPlatform.CodeAnalysis.Editor.CSharp.AutomaticCompletion.Sessions
{
    internal class StringLiteralCompletionSession : AbstractTokenBraceCompletionSession
    {
        private const char VerbatimStringPrefix = '@';

        public StringLiteralCompletionSession(ISyntaxFactsService syntaxFactsService)
            : base(syntaxFactsService, (int)SyntaxKind.StringLiteralToken, (int)SyntaxKind.StringLiteralToken)
        {
        }

        public override bool CheckOpeningPoint(IBraceCompletionSession session, CancellationToken cancellationToken)
        {
            var snapshot = session.SubjectBuffer.CurrentSnapshot;
            var position = session.OpeningPoint.GetPosition(snapshot);
            var token = snapshot.FindToken(position, cancellationToken);

            if (!IsValidToken(token) || token.RawKind != OpeningTokenKind)
            {
                return false;
            }

            if (token.SpanStart == position)
            {
                return true;
            }

            return token.SpanStart + 1 == position && snapshot[token.SpanStart] == VerbatimStringPrefix;
        }

        public override bool AllowOverType(IBraceCompletionSession session, CancellationToken cancellationToken)
        {
            return true;
        }
    }
}
