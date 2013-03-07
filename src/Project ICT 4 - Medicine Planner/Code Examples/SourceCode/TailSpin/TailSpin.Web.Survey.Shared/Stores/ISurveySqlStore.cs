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
    using Models;

    public interface ISurveySqlStore
    {
        void SaveSurvey(string connectionString, SurveyData surveyData);
        void Reset(string connectionString, string tenant, string slugName);
    }
}