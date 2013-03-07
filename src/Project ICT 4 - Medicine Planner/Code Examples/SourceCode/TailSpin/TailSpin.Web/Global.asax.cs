//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Web
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.IdentityModel.Web;
    using Microsoft.IdentityModel.Web.Configuration;
    using Microsoft.Practices.Unity;
    using TailSpin.Web.Controllers;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var container = new UnityContainer();
            ContainerBootstraper.RegisterTypes(container);
            ControllerBuilder.Current.SetControllerFactory(new UnityControllerFactory(container));

            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            AreaRegistration.RegisterAllAreas();
            AppRoutes.RegisterRoutes(RouteTable.Routes);

            FederatedAuthentication.ServiceConfigurationCreated += OnServiceConfigurationCreated;
        }

        private static void OnServiceConfigurationCreated(object sender, ServiceConfigurationCreatedEventArgs e)
        {
            // Use the <serviceCertificate> to protect the cookies that are
            // sent to the client.
            var sessionTransforms =
                new List<CookieTransform>(
                    new CookieTransform[] 
                    {
                        new DeflateCookieTransform(), 
                        new RsaEncryptionCookieTransform(e.ServiceConfiguration.ServiceCertificate),
                        new RsaSignatureCookieTransform(e.ServiceConfiguration.ServiceCertificate)  
                    });
            var sessionHandler = new SessionSecurityTokenHandler(sessionTransforms.AsReadOnly());
            e.ServiceConfiguration.SecurityTokenHandlers.AddOrReplace(sessionHandler);
        }

        private void Application_Error(object sender, System.EventArgs e)
        {
            System.Exception ex = Server.GetLastError();

            if (ex is System.Web.HttpRequestValidationException)
            {
            }
        }
    }
}