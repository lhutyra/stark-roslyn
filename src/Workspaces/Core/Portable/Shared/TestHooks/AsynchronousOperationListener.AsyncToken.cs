﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Threading;

namespace StarkPlatform.CodeAnalysis.Shared.TestHooks
{
    internal partial class AsynchronousOperationListener
    {
        internal class AsyncToken : IAsyncToken
        {
            private readonly AsynchronousOperationListener _listener;

            private bool _disposed;

            public AsyncToken(AsynchronousOperationListener listener)
            {
                _listener = listener;

                listener.Increment_NoLock();
            }

            public void Dispose()
            {
                using (_listener._gate.DisposableWait(CancellationToken.None))
                {
                    if (_disposed)
                    {
                        throw new InvalidOperationException("Double disposing of an async token");
                    }

                    _disposed = true;
                    _listener.Decrement_NoLock(this);
                }
            }
        }
    }
}
