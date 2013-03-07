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
    using System.ServiceModel.Web;
    using Microsoft.Practices.Unity;

    public class CustomServiceHost : WebServiceHost
    {
        private readonly IUnityContainer container;

        public CustomServiceHost(Type serviceType, Uri[] baseAddresses, IUnityContainer container) 
            : base(serviceType, baseAddresses)
        {
            this.container = container;
        }

        protected override void OnOpening()
        {
            if (Description.Behaviors.Find<CustomServiceBehavior>() == null)
            {
                Description.Behaviors.Add(new CustomServiceBehavior(this.container));
            }

            base.OnOpening();
        }
    }
}