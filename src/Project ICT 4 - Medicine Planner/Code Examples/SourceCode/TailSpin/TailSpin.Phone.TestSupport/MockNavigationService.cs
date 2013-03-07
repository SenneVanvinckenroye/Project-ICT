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
using TailSpin.PhoneClient.Adapters;

namespace TailSpin.Phone.TestSupport
{
    public class MockNavigationService : INavigationService
    {
        public MockNavigationService()
        {
            this.CanGoBack = true;
            this.GoBackTestCallback = () => { };
            this.DoesPageNeedToRecoverFromTombstoningTestCallback = (uri) => true;
            this.UpdateTombstonedPageTrackingTestCallback = (uri) => { };
        }

        public Action GoBackTestCallback { get; set; }

        public bool CanGoBack { get; set; }

        public bool Navigate(Uri destination)
        {
            Navigating(this, new NavigatingCancelEventArgs(destination, NavigationMode, IsCancelable, IsNavigationInitiator));
            Navigated(this, new NavigationEventArgs(null, destination, NavigationMode, IsNavigationInitiator));
            return true;
        }

        public void GoBack()
        {
            this.GoBackTestCallback();
        }

        public event NavigatedEventHandler Navigated;
        public event NavigatingCancelEventHandler Navigating;
        public event EventHandler<ObscuredEventArgs> Obscured;

        public bool RecoveredFromTombstoning { get; set; }

        public Action<Uri> UpdateTombstonedPageTrackingTestCallback { get; set; }
        public void UpdateTombstonedPageTracking(Uri pageUri)
        {
            UpdateTombstonedPageTrackingTestCallback(pageUri);
        }

        public Func<Uri, bool> DoesPageNeedToRecoverFromTombstoningTestCallback { get; set; }
        public bool DoesPageNeedtoRecoverFromTombstoning(Uri pageUri)
        {
            return DoesPageNeedToRecoverFromTombstoningTestCallback(pageUri);
        }

        public NavigationMode NavigationMode { get; set; }
        public bool IsCancelable { get; set; }
        public bool IsNavigationInitiator { get; set; }

        public void RaiseObscured()
        {
            Obscured(this, null);
        }
    }
}
