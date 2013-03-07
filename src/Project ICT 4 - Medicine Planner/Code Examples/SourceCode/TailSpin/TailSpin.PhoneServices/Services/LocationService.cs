//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using System.Device.Location;
using Microsoft.Phone.Reactive;
using TailSpin.PhoneServices.Services.Stores;
using TailSpin.Phone.Adapters;

namespace TailSpin.PhoneServices.Services
{
    public class LocationService : ILocationService
    {
        private readonly TimeSpan maximumAge = TimeSpan.FromMinutes(15);
        private readonly ISettingsStore settingsStore;
        private GeoCoordinate lastCoordinate = GeoCoordinate.Unknown;
        private DateTime lastCoordinateTime;
        private IGeoCoordinateWatcher geoCoordinateWatcher;

        /// <summary>
        /// This class uses the GeoCoordinateWatcher to determine the current location.
        /// To further reduce battery consumption, the MovementThreshold could be set to reduce the 
        /// frequency of the PositionChanged event being fired and the Position property being updated.
        /// </summary>
        /// <param name="settingsStore"></param>
        /// <param name="geoCoordinateWatcher"></param>
        public LocationService(ISettingsStore settingsStore, IGeoCoordinateWatcher geoCoordinateWatcher)
        {
            this.settingsStore = settingsStore;
            this.geoCoordinateWatcher = geoCoordinateWatcher;
        }

        public GeoCoordinate TryToGetCurrentLocation()
        {
            if (!settingsStore.LocationServiceAllowed)
            {
                return GeoCoordinate.Unknown;
            }

            if (geoCoordinateWatcher.Status == GeoPositionStatus.Ready)
            {
                lastCoordinate = geoCoordinateWatcher.Position.Location;
                lastCoordinateTime = geoCoordinateWatcher.Position.Timestamp.DateTime;
                return lastCoordinate;
            }

            if (maximumAge < (DateTime.Now - lastCoordinateTime))
            {
                return GeoCoordinate.Unknown;
            }
            else
            {
                return lastCoordinate;
            }
        }


        public void StartWatcher()
        {
            this.geoCoordinateWatcher.Start();
        }

        public void StopWatcher()
        {
            this.geoCoordinateWatcher.Stop();
        }
    }
}