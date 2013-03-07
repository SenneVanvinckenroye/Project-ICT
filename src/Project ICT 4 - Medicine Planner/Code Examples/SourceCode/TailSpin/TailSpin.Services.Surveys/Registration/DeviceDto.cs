//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System.Runtime.Serialization;

namespace TailSpin.Services.Surveys.Registration
{
    [DataContract]
    public class DeviceDto
    {
        [DataMember]
        public string Uri { get; set; }

        [DataMember]
        public bool RecieveNotifications { get; set; }
    }
}