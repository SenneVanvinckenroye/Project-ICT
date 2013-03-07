//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Web.Survey.Shared.SurveysFiltering
{
    using System;
    using System.Collections.Generic;
    using TailSpin.Web.Survey.Shared.Models;

    public interface IFilteringService
    {
        IEnumerable<Survey> GetSurveysForUser(string username, DateTime fromDate);
        IEnumerable<string> GetUsersForSurvey(Survey survey);
    }
}