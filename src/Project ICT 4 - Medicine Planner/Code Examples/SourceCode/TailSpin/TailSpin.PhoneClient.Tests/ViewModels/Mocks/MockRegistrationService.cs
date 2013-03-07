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
using TailSpin.PhoneClient.Services.RegistrationService;
using TailSpin.PhoneServices.Models;
using TailSpin.PhoneServices.Services;

namespace TailSpin.PhoneClient.Tests.ViewModels.Mocks
{
    public class MockRegistrationService : IRegistrationServiceClient
    {
        public bool CredentialsAreInvalid()
        {
            return true;
        }

        public void CloseChannel()
        {
        }

        public bool IsProcessing { get; set; }
        public event EventHandler IsProcessingChanged;

        public IObservable<TaskSummaryResult> UpdateReceiveNotifications(bool receiveNotifications)
        {
            return Observable.Return(TaskSummaryResult.Success);
        }

        public IObservable<Unit> UpdateTenants(IEnumerable<TenantItem> tenants)
        {
            return Observable.Empty<Unit>();
        }

        public IObservable<SurveyFiltersInformation> GetSurveysFilterInformation()
        {
            return Observable.Empty<SurveyFiltersInformation>();
        }
    }
}