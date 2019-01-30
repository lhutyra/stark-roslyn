// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.Notification;
using Microsoft.VisualStudio.ComponentModelHost;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.Options;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.Options.Style;

namespace StarkPlatform.VisualStudio.LanguageServices.CSharp.Options
{
    [Guid(Guids.StarkOptionPageNamingStyleIdString)]
    internal class NamingStylesOptionPage : AbstractOptionPage
    {
        private NamingStyleOptionPageControl _grid;
        private INotificationService _notificationService;

        protected override AbstractOptionPageControl CreateOptionPage(IServiceProvider serviceProvider)
        {
            var componentModel = (IComponentModel)serviceProvider.GetService(typeof(SComponentModel));
            var workspace = componentModel.GetService<VisualStudioWorkspace>();
            _notificationService = workspace.Services.GetService<INotificationService>();

            _grid = new NamingStyleOptionPageControl(serviceProvider, _notificationService, LanguageNames.Stark);
            return _grid;
        }

        protected override void OnDeactivate(CancelEventArgs e)
        {
            if (_grid.ContainsErrors())
            {
                e.Cancel = true;
                _notificationService.SendNotification(ServicesVSResources.Some_naming_rules_are_incomplete_Please_complete_or_remove_them);
            }

            base.OnDeactivate(e);
        }
    }
}
