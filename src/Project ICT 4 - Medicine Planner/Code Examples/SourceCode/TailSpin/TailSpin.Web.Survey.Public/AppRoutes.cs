//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Web.Survey.Public
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class AppRoutes
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "Home",
                string.Empty,
                new { controller = "Surveys", action = "Index" });

            routes.MapRoute(
                "ViewSurvey",
                "survey/{tenant}/{surveySlug}",
                new { controller = "Surveys", action = "Display" });

            routes.MapRoute(
                "ThankYouForFillingTheSurvey",
                "survey/{tenant}/{surveySlug}/thankyou",
                new { controller = "Surveys", action = "ThankYou" });
        }
    }
}
