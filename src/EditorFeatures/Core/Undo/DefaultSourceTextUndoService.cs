﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Composition;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.CodeAnalysis.Text;
using Microsoft.VisualStudio.Text;

namespace StarkPlatform.CodeAnalysis.Editor.Undo
{
    [ExportWorkspaceService(typeof(ISourceTextUndoService), ServiceLayer.Default), Shared]
    internal sealed class DefaultSourceTextUndoService : ISourceTextUndoService
    {
        public ISourceTextUndoTransaction RegisterUndoTransaction(SourceText sourceText, string description)
        {
            return null;
        }

        public bool BeginUndoTransaction(ITextSnapshot snapshot)
        {
            return false;
        }

        public bool EndUndoTransaction(ISourceTextUndoTransaction transaction)
        {
            return false;
        }
    }
}
