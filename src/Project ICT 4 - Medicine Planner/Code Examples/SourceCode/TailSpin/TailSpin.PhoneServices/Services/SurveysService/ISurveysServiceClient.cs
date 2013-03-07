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

namespace TailSpin.PhoneServices.Services.SurveysService
{
    public interface ISurveysServiceClient
    {
        IObservable<IEnumerable<SurveyTemplate>> GetNewSurveys(string lastSyncDate);
        IObservable<Unit> SaveSurveyAnswers(IEnumerable<SurveyAnswer> surveyAnswers);
    }
}
