//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Web.Survey.Shared.Stores
{
    using System;
    using System.Collections.Generic;
    using TailSpin.Web.Survey.Shared.Models;

    public interface ISurveyStore
    {
        void Initialize();
        void SaveSurvey(Survey survey);
        void DeleteSurveyByTenantAndSlugName(string tenant, string slugName);
        Survey GetSurveyByTenantAndSlugName(string tenant, string slugName, bool getQuestions);
        IEnumerable<Survey> GetSurveysByTenant(string tenant);
        IEnumerable<Survey> GetRecentSurveys();
        IEnumerable<Survey> GetSurveys(string tenant, DateTime fromDate);
    }
}