//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using Microsoft.Phone.Reactive;
using TailSpin.PhoneServices.Services;
using TailSpin.PhoneServices.Services.Stores;

namespace TailSpin.PhoneClient.Tests.ViewModels.Mocks
{
    public class SurveysSynchronizationServiceMock : ISurveysSynchronizationService
    {
        public IObservable<TaskCompletedSummary> GetNewSurveys()
        {
            return Observable.Empty<TaskCompletedSummary>();
        }

        public IObservable<TaskCompletedSummary> GetNewSurveys(ISurveyStore surveyStore)
        {
            return Observable.Empty<TaskCompletedSummary>();
        }

        public IObservable<TaskCompletedSummary[]> StartSynchronization()
        {
            return Observable.Empty<TaskCompletedSummary[]>();
        }

        public IObservable<TaskCompletedSummary> UploadSurveys()
        {
            return Observable.Empty<TaskCompletedSummary>();
        }

        public IObservable<TaskCompletedSummary> UploadSurveys(ISurveyStore surveyStore)
        {
            return Observable.Empty<TaskCompletedSummary>();
        }

        public int UnopenedSurveyCount { get; set; }
    }
}
