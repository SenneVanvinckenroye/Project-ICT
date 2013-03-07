//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using Microsoft.Phone.Notification;
using Microsoft.Phone.Reactive;

namespace TailSpin.PhoneClient.Services.RegistrationService
{
    public static class HttpNotificationChannelExtensions
    {
        public static IObservable<IEvent<NotificationChannelUriEventArgs>> ObserveChannelUriUpdatedEvent(this HttpNotificationChannel httpNotificationChannel)
        {
            return Observable.FromEvent<NotificationChannelUriEventArgs>(
                h => httpNotificationChannel.ChannelUriUpdated += h,
                h => httpNotificationChannel.ChannelUriUpdated -= h);
        }

        public static IObservable<IEvent<NotificationChannelErrorEventArgs>> ObserveErrorOccurredEvent(this HttpNotificationChannel httpNotificationChannel)
        {
            return Observable.FromEvent<NotificationChannelErrorEventArgs>(
                h => httpNotificationChannel.ErrorOccurred += h,
                h => httpNotificationChannel.ErrorOccurred -= h);
        }



        /// <summary>
        /// Tailspin does not currently do anything in the event that a push notification 
        /// is received while the application is in the foreground.
        /// This could be a good time to notify the user or update the UI.
        /// </summary>
        /// <param name="httpNotificationChannel"></param>
        /// <returns></returns>
        public static IObservable<IEvent<NotificationEventArgs>> ObserveShellToastNotificationReceivedEvent(this HttpNotificationChannel httpNotificationChannel)
        {
            return Observable.FromEvent<NotificationEventArgs>(
                h => httpNotificationChannel.ShellToastNotificationReceived += h,
                h => httpNotificationChannel.ShellToastNotificationReceived -= h);
        }
    }
}
