//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Xna.Framework;

namespace TailSpin.PhoneClient.Infrastructure
{
    public class XnaAsyncDispatcher
    {
        private readonly DispatcherTimer frameworkDispatcherTimer;

        public XnaAsyncDispatcher(TimeSpan dispatchInterval)
        {
            this.frameworkDispatcherTimer = new DispatcherTimer();
            this.frameworkDispatcherTimer.Tick += (s, e) => FrameworkDispatcher.Update();
            this.frameworkDispatcherTimer.Interval = dispatchInterval;
        }

        public void StartService()
        {
            this.frameworkDispatcherTimer.Start();
        }

        public void StopService()
        {
            this.frameworkDispatcherTimer.Stop();
        }
    }
}
