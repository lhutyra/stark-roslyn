﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using StarkPlatform.CodeAnalysis.Formatting.Rules;

namespace StarkPlatform.CodeAnalysis.Formatting
{
    /// <summary>
    /// a delegate cache for a continuation style chaining
    /// </summary>
    internal class OperationCache<TResult> : IOperationHolder<TResult>
    {
        public OperationCache(
            Func<int, SyntaxToken, SyntaxToken, NextOperation<TResult>, TResult> nextOperation,
            Func<int, SyntaxToken, SyntaxToken, IOperationHolder<TResult>, TResult> continuation)
        {
            this.NextOperation = nextOperation;
            this.Continuation = continuation;
        }

        public Func<int, SyntaxToken, SyntaxToken, NextOperation<TResult>, TResult> NextOperation { get; }
        public Func<int, SyntaxToken, SyntaxToken, IOperationHolder<TResult>, TResult> Continuation { get; }
    }
}
