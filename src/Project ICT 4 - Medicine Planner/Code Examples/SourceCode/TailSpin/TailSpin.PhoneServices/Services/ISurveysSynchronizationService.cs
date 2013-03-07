//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using TailSpin.PhoneServices.Services.Stores;

namespace TailSpin.PhoneServices.Services
{
    public interface ISurveysSynchronizationService
    {
        IObservable<TaskCompletedSummary> GetNewSurveys();
        IObservable<TaskCompletedSummary> GetNewSurveys(ISurveyStore surveyStore);
        IObservable<TaskCompletedSummary[]> StartSynchronization();
        IObservable<TaskCompletedSummary> UploadSurveys();
        IObservable<TaskCompletedSummary> UploadSurveys(ISurveyStore surveyStore);
        int UnopenedSurveyCount { get; set; }
    }
}
