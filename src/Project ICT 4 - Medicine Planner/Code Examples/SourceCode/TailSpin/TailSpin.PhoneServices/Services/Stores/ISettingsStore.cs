//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;

namespace TailSpin.PhoneServices.Services.Stores
{
    public interface ISettingsStore
    {
        string Password { get; set; }
        string UserName { get; set; }
        bool SubscribeToPushNotifications { get; set; }
        bool LocationServiceAllowed { get; set; }
        bool BackgroundTasksAllowed { get; set; }
        event EventHandler UserChanged;
    }
}
