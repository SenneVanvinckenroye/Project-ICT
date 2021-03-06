﻿//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;

namespace TailSpin.PhoneClient.Adapters
{
    public interface ICameraCaptureTask
    {
        SettablePhotoResult TaskEventArgs
        {
            get;
            set;
        }

        event EventHandler<SettablePhotoResult> Completed;

        void Show();
    }
}
