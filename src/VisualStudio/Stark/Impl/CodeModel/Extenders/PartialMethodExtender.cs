// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Runtime.InteropServices;
using StarkPlatform.VisualStudio.LanguageServices.CSharp.CodeModel.Interop;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.Interop;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.CodeModel.Extenders
{
    [ComVisible(true)]
    [ComDefaultInterface(typeof(ICSPartialMethodExtender))]
    public class PartialMethodExtender : ICSPartialMethodExtender
    {
        internal static ICSPartialMethodExtender Create(bool isPartial, bool isDeclaration, bool hasOtherPart)
        {
            var result = new PartialMethodExtender(isPartial, isDeclaration, hasOtherPart);
            return (ICSPartialMethodExtender)ComAggregate.CreateAggregatedObject(result);
        }

        private readonly bool _isPartial;
        private readonly bool _isDeclaration;
        private readonly bool _hasOtherPart;

        private PartialMethodExtender(bool isPartial, bool isDeclaration, bool hasOtherPart)
        {
            _isPartial = isPartial;
            _isDeclaration = isDeclaration;
            _hasOtherPart = hasOtherPart;
        }

        public bool IsPartial
        {
            get { return _isPartial; }
        }

        public bool IsDeclaration
        {
            get { return _isDeclaration; }
        }

        public bool HasOtherPart
        {
            get { return _hasOtherPart; }
        }
    }
}
