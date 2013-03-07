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
using System.Threading;

using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TailSpin.Phone.TestSupport;
using TailSpin.PhoneClient.Tests.ViewModels.Mocks;
using TailSpin.PhoneServices.Services;
using System.Windows.Threading;

namespace TailSpin.PhoneClient.Tests.Services
{
    [TestClass]
    public class LocationServiceFixture : SilverlightTest
    {
        [TestMethod]
        public void TryToGetCurrentLocationReturnsLocation()
        {
            var settingStore = new MockSettingsStore { LocationServiceAllowed = true };
            var geoCoordinateWatcher = new MockGeoCoordinateWatcher();
            
            var target = new LocationService(settingStore, geoCoordinateWatcher);
            target.StartWatcher();

            geoCoordinateWatcher.ChangePosition(new GeoCoordinate(12, 34), DateTimeOffset.Now);

            var coordinates = target.TryToGetCurrentLocation();
            Assert.AreEqual(12, coordinates.Latitude);
            Assert.AreEqual(34, coordinates.Longitude);
        }

        [TestMethod]
        public void TryToGetCurrentLocationReturnsUnknownLocationWhenAppSettingNoLocationService()
        {
            var settingStore = new MockSettingsStore { LocationServiceAllowed = false };
            var geoCoordinateWatcher = new MockGeoCoordinateWatcher();

            var target = new LocationService(settingStore, geoCoordinateWatcher);
            target.StartWatcher();

            geoCoordinateWatcher.ChangePosition(new GeoCoordinate(12, 34), DateTimeOffset.Now);

            var coordinates = target.TryToGetCurrentLocation();
            Assert.AreEqual(GeoCoordinate.Unknown, coordinates);
        }

        [TestMethod]
        public void TryToGetCurrentLocationReturnsLastLocationIfWatcherNotReady()
        {
            var settingStore = new MockSettingsStore { LocationServiceAllowed = true };
            var geoCoordinateWatcher = new MockGeoCoordinateWatcher();

            var target = new LocationService(settingStore, geoCoordinateWatcher);
            target.StartWatcher();

            geoCoordinateWatcher.ChangePosition(new GeoCoordinate(12, 34), DateTimeOffset.Now);
            target.TryToGetCurrentLocation();

            geoCoordinateWatcher.Status = GeoPositionStatus.Initializing;
            var coordinates = target.TryToGetCurrentLocation();
            
            Assert.AreEqual(12, coordinates.Latitude);
            Assert.AreEqual(34, coordinates.Longitude);
        }

        [TestMethod]
        public void TryToGetCurrentLocationReturnsUnknownLocationWhenLastLocationTooOld()
        {
            var settingStore = new MockSettingsStore { LocationServiceAllowed = true };
            var geoCoordinateWatcher = new MockGeoCoordinateWatcher();

            var target = new LocationService(settingStore, geoCoordinateWatcher);
            target.StartWatcher();

            geoCoordinateWatcher.ChangePosition(new GeoCoordinate(12, 34), DateTimeOffset.Now.AddHours(-1));
            geoCoordinateWatcher.Status = GeoPositionStatus.Initializing;
            var coordinates = target.TryToGetCurrentLocation();

            Assert.AreEqual(GeoCoordinate.Unknown, coordinates);
        }
    }
}

