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
    using AzureStorage;
    using TailSpin.Web.Survey.Shared.Models;

    public interface ISurveyAnswerContainerFactory
    {
        IAzureBlobContainer<SurveyAnswer> Create(string tenant, string surveySlug);
    }
}