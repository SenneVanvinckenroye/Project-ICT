//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using TailSpin.PhoneServices.Services.Stores;

namespace TailSpin.PhoneClient.Tests.ViewModels.Mocks
{
    public class MockSettingsStore : ISettingsStore
    {
        public string Password { get; set; }
        public bool SubscribeToPushNotifications { get; set; }
        public string UserName { get; set; }
        public bool BackgroundTasksAllowed { get; set; }
        public bool LocationServiceAllowed { get; set; }
        public event EventHandler UserChanged;
    }
}