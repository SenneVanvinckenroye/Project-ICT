//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;

using TailSpin.PhoneServices.Services;
using System.Device.Location;

namespace TailSpin.Phone.TestSupport
{
    public class MockLocationService : ILocationService
    {
        public GeoCoordinate TryToGetCurrentLocation()
        {
            return default(GeoCoordinate);
        }

        public void StartWatcher()
        {
        }

        public void StopWatcher()
        {
        }
    }
}