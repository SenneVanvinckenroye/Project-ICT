//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Phone.Notification;
using Microsoft.Phone.Reactive;
using TailSpin.Phone.Adapters;
using TailSpin.PhoneServices.Models;
using TailSpin.PhoneServices.Services;
using TailSpin.PhoneServices.Services.Clients;
using TailSpin.PhoneServices.Services.Stores;
using TailSpin.Services.Surveys.Registration;

namespace TailSpin.PhoneClient.Services.RegistrationService
{
    public class RegistrationServiceClient : IRegistrationServiceClient
    {
        private const string ChannelName = "tailspindemo.cloudapp.net";
        private const string ServiceName = "TailSpinRegistrationService";
        private readonly Uri serviceUri;
        private readonly ISettingsStore settingsStore;
        private readonly IHttpClient httpClient;
        private HttpNotificationChannel httpChannel;

        public RegistrationServiceClient(Uri serviceUri, ISettingsStore settingsStore, IHttpClient httpClient)
        {
            this.serviceUri = serviceUri;
            this.settingsStore = settingsStore;
            this.httpClient = httpClient;
        }

        public bool CredentialsAreInvalid()
        {
            var currentUser = settingsStore.UserName.ToLower(CultureInfo.InvariantCulture);
            return currentUser != "fred"
                && currentUser != "joe"
                && currentUser != "scott";
        }

        public IObservable<SurveyFiltersInformation> GetSurveysFilterInformation()
        {
            var uri = new Uri(serviceUri, "GetFilters");
            return
                httpClient
                    .GetJson<SurveyFiltersInformationDto>(new HttpWebRequestAdapter(uri), settingsStore.UserName,
                                                          settingsStore.Password)
                    .Select(
                        surveyFiltersInformationDto =>
                        new SurveyFiltersInformation
                        {
                            AllTenants =
                                surveyFiltersInformationDto.AllTenants
                                .Select(t => new TenantItem { Key = t.Key, Name = t.Name }),
                            SelectedTenants =
                                surveyFiltersInformationDto.SurveyFilters.Tenants
                                .Select(t => new TenantItem { Key = t.Key, Name = t.Name })
                        });
        }

        public IObservable<Unit> UpdateTenants(IEnumerable<TenantItem> tenants)
        {
            var surveyFiltersDto = new SurveyFiltersDto { Tenants = tenants.Select(t => new TenantDto { Key = t.Key }).ToArray() };

            var uri = new Uri(serviceUri, "SetFilters");
            return
                httpClient
                    .PostJson(new HttpWebRequestAdapter(uri), settingsStore.UserName, settingsStore.Password,
                              surveyFiltersDto);
        }


        public IObservable<TaskSummaryResult> UpdateReceiveNotifications(bool receiveNotifications)
        {
            IsProcessing = true;

            if (receiveNotifications)
            {
                //Create channel and hook up to its events.
                httpChannel = new HttpNotificationChannel(ChannelName, ServiceName);

                var channelUriUpdated = from evt in httpChannel.ObserveChannelUriUpdatedEvent()
                                        from result in
                                            BindChannelAndUpdateDeviceUriInService(receiveNotifications,
                                                                                    evt.EventArgs.ChannelUri)
                                        select result;

                var channelUriUpdateFail =
                     from o in httpChannel.ObserveErrorOccurredEvent()
                     select TaskSummaryResult.UnknownError;

                httpChannel.Open();

                // If the notification service does not respond in time, it is assumed that the server is unreachable.
                // The first event that happens is returned.

                return channelUriUpdated.Timeout(TimeSpan.FromSeconds(60)).Amb(channelUriUpdateFail).Take(1)
                    .Select(tsr =>
                                {
                                    IsProcessing = false;
                                    return tsr;
                                })
                    .Catch<TaskSummaryResult, TimeoutException>(
                        (e) =>
                            {
                                IsProcessing = false;
                                return Observable.Return(TaskSummaryResult.UnreachableServer);
                            });
            }
            else
            {
                // If channel is already openned, close and dispose
                httpChannel = HttpNotificationChannel.Find(ChannelName);

                if (httpChannel != null && httpChannel.ChannelUri != null)
                {
                    return BindChannelAndUpdateDeviceUriInService(receiveNotifications, httpChannel.ChannelUri)
                        .Select(taskSummary =>
                                    {
                                        IsProcessing = false;
                                        return TaskSummaryResult.Success;
                                    });
                }
                else
                {
                    IsProcessing = false;
                    return Observable.Return(TaskSummaryResult.Success);                    
                }
            }
        }

        private bool isProcessing;
        public bool IsProcessing
        {
            get { return isProcessing; }
            private set
            {
                isProcessing = value;
                var handler = this.IsProcessingChanged;
                if (handler != null)
                {
                    handler(this, null);
                }
            }
        }

        public event EventHandler IsProcessingChanged;

        public void CloseChannel()
        {
            if (httpChannel != null)
            {
                this.httpChannel.Close();
                this.httpChannel.Dispose();
                this.httpChannel = null;
            }
        }

        private IObservable<TaskSummaryResult> BindChannelAndUpdateDeviceUriInService(bool receiveNotifications, Uri channelUri)
        {
            var device = new DeviceDto
            {
                Uri = channelUri != null ? channelUri.ToString() : string.Empty,
                RecieveNotifications = receiveNotifications
            };

            var uri = new Uri(serviceUri, "Notifications");

            return httpClient
                .PostJson(new HttpWebRequestAdapter(uri), settingsStore.UserName, settingsStore.Password,
                            device).Select(u =>
                            {
                                BindChannelAndNotify(receiveNotifications);
                                return TaskSummaryResult.Success;
                            });

        }

        private void BindChannelAndNotify(bool receiveNotifications)
        {
            if (httpChannel != null)
            {
                if (receiveNotifications)
                {
                    if (!httpChannel.IsShellToastBound)
                        httpChannel.BindToShellToast();

                    if (!httpChannel.IsShellTileBound)
                        httpChannel.BindToShellTile();
                }
                else
                {
                    if (httpChannel.IsShellToastBound)
                        httpChannel.UnbindToShellToast();

                    if (httpChannel.IsShellTileBound)
                        httpChannel.UnbindToShellTile();
                }
            }
        }
    }
}

