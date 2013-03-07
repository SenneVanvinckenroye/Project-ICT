//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Web.Survey.Shared.Notifications
{
    public delegate void DeviceNotFoundInMpns(string channelUri);

    public interface IPushNotification
    {
        void PushTileNotification(string channelUri, string message, string backgroundImage, int count, DeviceNotFoundInMpns callback);
        void PushToastNotification(string channelUri, string text1, string text2, DeviceNotFoundInMpns callback);
    }
}