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

namespace TailSpin.Phone.Adapters
{
    public interface IGeoCoordinateWatcher : IDisposable
    {
        void Start();
        void Start(bool suppressPermissionPrompt);
        bool TryStart(bool suppressPermissionPrompt, TimeSpan timeout);
        void Stop();
        GeoPositionAccuracy DesiredAccuracy { get; }
        double MovementThreshold { get; set; }
        GeoPosition<GeoCoordinate> Position { get; }
        GeoPositionStatus Status { get; }
        GeoPositionPermission Permission { get; }
        event EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>> PositionChanged;
        event EventHandler<GeoPositionStatusChangedEventArgs> StatusChanged;
    }
}