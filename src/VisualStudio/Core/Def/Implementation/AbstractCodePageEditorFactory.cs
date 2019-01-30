﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation
{
    internal abstract class AbstractCodePageEditorFactory : IVsEditorFactory
    {
        private AbstractEditorFactory _editorFactory;

        protected AbstractCodePageEditorFactory(AbstractEditorFactory editorFactory)
        {
            _editorFactory = editorFactory;
        }

        int IVsEditorFactory.CreateEditorInstance(
            uint grfCreateDoc,
            string pszMkDocument,
            string pszPhysicalView,
            IVsHierarchy vsHierarchy,
            uint itemid,
            IntPtr punkDocDataExisting,
            out IntPtr ppunkDocView,
            out IntPtr ppunkDocData,
            out string pbstrEditorCaption,
            out Guid pguidCmdUI,
            out int pgrfCDW)
        {
            if (punkDocDataExisting != IntPtr.Zero)
            {
                ppunkDocView = IntPtr.Zero;
                ppunkDocData = IntPtr.Zero;
                pbstrEditorCaption = null;
                pguidCmdUI = Guid.Empty;
                pgrfCDW = 0;

                return VSConstants.VS_E_INCOMPATIBLEDOCDATA;
            }

            _editorFactory.SetEncoding(true);
            try
            {
                return _editorFactory.CreateEditorInstance(
                    grfCreateDoc, pszMkDocument, pszPhysicalView, vsHierarchy, itemid,
                    punkDocDataExisting, out ppunkDocView, out ppunkDocData,
                    out pbstrEditorCaption, out pguidCmdUI, out pgrfCDW);
            }
            finally
            {
                _editorFactory.SetEncoding(false);
            }
        }

        int IVsEditorFactory.MapLogicalView(ref Guid rguidLogicalView, out string pbstrPhysicalView)
        {
            return _editorFactory.MapLogicalView(ref rguidLogicalView, out pbstrPhysicalView);
        }

        int IVsEditorFactory.SetSite(IOleServiceProvider psp)
        {
            return VSConstants.S_OK;
        }

        int IVsEditorFactory.Close()
        {
            return VSConstants.S_OK;
        }
    }
}
