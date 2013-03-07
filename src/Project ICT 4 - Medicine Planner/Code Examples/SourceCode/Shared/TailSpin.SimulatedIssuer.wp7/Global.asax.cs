//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.SimulatedIssuer
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("*.svc");
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("*.htm");

            routes.MapRoute(
                "SignInResponse",
                "SignInResponse",
                new { controller = "Issuer", action = "SignInResponse" });
            
            routes.MapRoute(
                "Default",
                string.Empty,
                new { controller = "Issuer", action = "Index" });
        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}