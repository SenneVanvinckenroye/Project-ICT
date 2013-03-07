//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using System.IO;
using Microsoft.Phone.Tasks;

namespace TailSpin.PhoneClient.Adapters
{
    public class SettablePhotoResult : PhotoResult
    {
        public SettablePhotoResult(PhotoResult photoResult)
        {
            ChosenPhoto = photoResult.ChosenPhoto;
            OriginalFileName = photoResult.OriginalFileName;
            Error = photoResult.Error;
        }

        public SettablePhotoResult()
        {
        }

        public new Stream ChosenPhoto { get; set; }
        public new string OriginalFileName { get; set; }
        public new Exception Error { get; set; }
    }
}
