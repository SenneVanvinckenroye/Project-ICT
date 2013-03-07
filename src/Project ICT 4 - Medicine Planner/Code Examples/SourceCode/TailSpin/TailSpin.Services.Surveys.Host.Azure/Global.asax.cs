//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Services.Surveys
{
    using System;
    using System.ServiceModel.Activation;
    using System.Web;
    using System.Web.Routing;
    using Registration;
    using TailSpin.Services.Surveys.Surveys;
    using TailSpin.Web.Survey.Shared;

    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            var container = ContainerLocator.Container;

            ComponentRegistration.RegisterSurveyStore(container);
            ComponentRegistration.RegisterSurveyAnswerStore(container);
            ComponentRegistration.RegisterMediaAnswerStore(container);
            ComponentRegistration.RegisterTenantStore(container);
            ComponentRegistration.RegisterTenantFilterStore(container);
            ComponentRegistration.RegisterUserDeviceStore(container);
            ComponentRegistration.RegisterFilteringService(container);

            RegisterRoutes();
        }

        private static void RegisterRoutes()
        {
            var customServiceHostFactory = new CustomServiceHostFactory(ContainerLocator.Container);
            RouteTable.Routes.Add(new ServiceRoute("Registration", customServiceHostFactory, typeof(RegistrationService)));
            RouteTable.Routes.Add(new ServiceRoute("Survey", customServiceHostFactory, typeof(SurveysService)));
        }
    }
}