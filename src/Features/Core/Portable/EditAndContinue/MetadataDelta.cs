﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace StarkPlatform.CodeAnalysis.EditAndContinue
{
    internal struct MetadataDelta
    {
        public readonly byte[] Bytes;

        public MetadataDelta(byte[] bytes)
        {
            this.Bytes = bytes;
        }
    }
}
