﻿//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class AppRoutes
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "OnBoarding",
                string.Empty,
                new { controller = "OnBoarding", action = "Index" });

            routes.MapRoute(
               "JoinTenant",
               "Join",
               new { controller = "OnBoarding", action = "Join" });

            routes.MapRoute(
                "FederationResultProcessing",
                "FederationResult",
                new { controller = "ClaimsAuthentication", action = "FederationResult" });

            routes.MapRoute(
                "FederatedSignout",
                "Signout",
                new { controller = "ClaimsAuthentication", action = "Signout" });

            routes.MapRoute(
                "MyAccount",
                "{tenant}/MyAccount",
                new { controller = "Account", action = "Index" });

            routes.MapRoute(
                "UploadLogo",
                "{tenant}/MyAccount/UploadLogo",
                new { controller = "Account", action = "UploadLogo" });
        }
    }
}
