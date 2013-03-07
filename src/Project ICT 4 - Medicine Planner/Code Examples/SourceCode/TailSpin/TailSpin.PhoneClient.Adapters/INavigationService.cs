//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using System.Collections.Generic;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace TailSpin.PhoneClient.Adapters
{
    public interface INavigationService
    {
        bool CanGoBack { get; }
        bool Navigate(Uri source);
        void GoBack();
        event NavigatedEventHandler Navigated;
        event NavigatingCancelEventHandler Navigating;
        event EventHandler<ObscuredEventArgs> Obscured;
        bool RecoveredFromTombstoning { get; set; }
        void UpdateTombstonedPageTracking(Uri pageUri);
        bool DoesPageNeedtoRecoverFromTombstoning(Uri pageUri);
    }
}
