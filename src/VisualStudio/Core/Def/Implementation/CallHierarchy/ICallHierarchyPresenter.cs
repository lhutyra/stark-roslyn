// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.CodeAnalysis.Editor.Implementation.CallHierarchy;

namespace StarkPlatform.CodeAnalysis.Editor.Host
{
    internal interface ICallHierarchyPresenter
    {
        void PresentRoot(CallHierarchyItem root);
    }
}
