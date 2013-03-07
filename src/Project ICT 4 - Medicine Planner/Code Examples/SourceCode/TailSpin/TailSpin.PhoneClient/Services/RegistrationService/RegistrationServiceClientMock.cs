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
using Microsoft.Phone.Reactive;
using TailSpin.PhoneServices.Models;
using TailSpin.PhoneServices.Services;
using TailSpin.PhoneServices.Services.Stores;

namespace TailSpin.PhoneClient.Services.RegistrationService
{
    public class RegistrationServiceClientMock : IRegistrationServiceClient
    {
        private readonly ISettingsStore settingsStore;

        public RegistrationServiceClientMock(ISettingsStore settingsStore)
        {
            this.settingsStore = settingsStore;
        }

        public bool CredentialsAreInvalid()
        {
            var currentUser = settingsStore.UserName.ToLower(CultureInfo.InvariantCulture);
            return currentUser != "fred"
                && currentUser != "joe"
                && currentUser != "scott";
        }

        public void CloseChannel()
        {
        }

        public bool IsProcessing
        {
            get { return false; }
        }

        public event EventHandler IsProcessingChanged;

        public IObservable<TaskSummaryResult> UpdateReceiveNotifications(bool receiveNotifications)
        {
            return Observable.Return(TaskSummaryResult.Success);
        }

        public IObservable<Unit> UpdateTenants(IEnumerable<TenantItem> tenants)
        {
            return Observable.Throw<Unit>(new InvalidOperationException("You can’t change the filter settings in PhoneOnly solution"));
        }

        public IObservable<SurveyFiltersInformation> GetSurveysFilterInformation()
        {
            if (CredentialsAreInvalid())
            {
                return Observable.Throw<SurveyFiltersInformation>(new UnauthorizedAccessException());
            }

            return Observable.Return(
                new SurveyFiltersInformation
                {
                    AllTenants = new[]
                                         {
                                             new TenantItem { Key = "adatum", Name = "Adatum" },
                                             new TenantItem { Key = "fabrikam", Name = "Fabrikam" }
                                         },
                    SelectedTenants = new TenantItem[0]
                });
        }
    }
}
