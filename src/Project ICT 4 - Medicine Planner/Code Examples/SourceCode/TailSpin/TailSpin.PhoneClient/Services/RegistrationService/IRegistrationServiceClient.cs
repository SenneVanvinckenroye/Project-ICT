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
using Microsoft.Phone.Reactive;
using TailSpin.PhoneServices.Models;
using TailSpin.PhoneServices.Services;

namespace TailSpin.PhoneClient.Services.RegistrationService
{
    public interface IRegistrationServiceClient
    {
        IObservable<TaskSummaryResult> UpdateReceiveNotifications(bool receiveNotifications);
        IObservable<Unit> UpdateTenants(IEnumerable<TenantItem> tenants);
        IObservable<SurveyFiltersInformation> GetSurveysFilterInformation();
        bool CredentialsAreInvalid();
        void CloseChannel();
        bool IsProcessing { get; }
        event EventHandler IsProcessingChanged;
    }
}
