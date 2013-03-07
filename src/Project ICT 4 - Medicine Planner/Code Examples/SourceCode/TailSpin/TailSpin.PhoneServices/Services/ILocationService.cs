//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System.Device.Location;

namespace TailSpin.PhoneServices.Services
{
    public interface ILocationService
    {
        GeoCoordinate TryToGetCurrentLocation();
        void StartWatcher();
        void StopWatcher();
    }
}
