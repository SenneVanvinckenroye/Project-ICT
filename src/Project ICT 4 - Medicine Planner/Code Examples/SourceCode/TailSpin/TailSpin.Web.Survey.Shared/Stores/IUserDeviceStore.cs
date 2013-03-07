//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Web.Survey.Shared.Stores
{
    using System;
    using System.Collections.Generic;

    public interface IUserDeviceStore
    {
        void Initialize();
        void SetUserDevice(string username, Uri deviceUri);
        void RemoveUserDevice(Uri deviceUri);
        IEnumerable<Uri> GetDevices(string username);
    }
}