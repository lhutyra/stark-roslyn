﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Runtime.InteropServices;
using System.Threading;
using StarkPlatform.CodeAnalysis.Editor.Host;
using StarkPlatform.CodeAnalysis.Notification;
using StarkPlatform.CodeAnalysis.Shared.Utilities;
using Microsoft.VisualStudio.Shell.Interop;
using Roslyn.Utilities;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.Utilities
{
    internal sealed partial class VisualStudioWaitContext : IWaitContext
    {
        private const int DelayToShowDialogSecs = 2;

        private readonly IVsThreadedWaitDialog3 _dialog;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly GlobalOperationRegistration _registration;

        private readonly string _title;
        private string _message;
        private bool _allowCancel;

        public IProgressTracker ProgressTracker { get; }

        public VisualStudioWaitContext(
            IGlobalOperationNotificationService notificationService,
            IVsThreadedWaitDialogFactory dialogFactory,
            string title,
            string message,
            bool allowCancel,
            bool showProgress)
        {
            _title = title;
            _message = message;
            _allowCancel = allowCancel;
            _cancellationTokenSource = new CancellationTokenSource();

            this.ProgressTracker = showProgress
                ? new ProgressTracker((_1, _2, _3) => UpdateDialog())
                : new ProgressTracker();

            _dialog = CreateDialog(dialogFactory, showProgress);
            _registration = notificationService.Start(title);
        }

        private IVsThreadedWaitDialog3 CreateDialog(
            IVsThreadedWaitDialogFactory dialogFactory, bool showProgress)
        {
            Marshal.ThrowExceptionForHR(dialogFactory.CreateInstance(out var dialog2));
            Contract.ThrowIfNull(dialog2);

            var dialog3 = (IVsThreadedWaitDialog3)dialog2;

            var callback = new Callback(this);

            dialog3.StartWaitDialogWithCallback(
                szWaitCaption: _title,
                szWaitMessage: this.ProgressTracker.Description ?? _message,
                szProgressText: null,
                varStatusBmpAnim: null,
                szStatusBarText: null,
                fIsCancelable: _allowCancel,
                iDelayToShowDialog: DelayToShowDialogSecs,
                fShowProgress: showProgress,
                iTotalSteps: this.ProgressTracker.TotalItems,
                iCurrentStep: this.ProgressTracker.CompletedItems,
                pCallback: callback);

            return dialog3;
        }

        public CancellationToken CancellationToken
        {
            get
            {
                return _allowCancel
                    ? _cancellationTokenSource.Token
                    : CancellationToken.None;
            }
        }

        public string Message
        {
            get
            {
                return _message;
            }

            set
            {
                _message = value;
                UpdateDialog();
            }
        }

        public bool AllowCancel
        {
            get
            {
                return _allowCancel;
            }

            set
            {
                _allowCancel = value;
                UpdateDialog();
            }
        }

        private void UpdateDialog()
        {
            _dialog.UpdateProgress(
                this.ProgressTracker.Description ?? _message,
                szProgressText: null,
                szStatusBarText: null,
                iCurrentStep: this.ProgressTracker.CompletedItems,
                iTotalSteps: this.ProgressTracker.TotalItems,
                fDisableCancel: !_allowCancel,
                pfCanceled: out var hasCancelled);
        }

        public void Dispose()
        {
            _dialog.EndWaitDialog(out var canceled);

            if (canceled == 0)
            {
                _registration.Done();
            }

            _registration.Dispose();
        }

        private void OnCanceled()
        {
            if (_allowCancel)
            {
                _cancellationTokenSource.Cancel();
            }
        }
    }
}
