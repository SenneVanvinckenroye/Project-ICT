//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using TailSpin.PhoneClient.Adapters;

namespace TailSpin.Phone.TestSupport
{
    public class MockCameraCaptureTask : ICameraCaptureTask
    {
        public SettablePhotoResult TaskEventArgs { get; set; }

        public Action ShowTestCallback { get; set; } 

        public event EventHandler<SettablePhotoResult> Completed;
        public void Show()
        {
            if (TaskEventArgs == null)
            {
                throw new InvalidOperationException("Must set TaskEventArgs prior to calling Show.");
            }

            if (ShowTestCallback != null) ShowTestCallback();

            var handler = Completed;
            if (handler != null)
                handler(null, TaskEventArgs);
        }
    }
}
