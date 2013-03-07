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
    using Microsoft.Practices.Unity;

    public static class ContainerLocator
    {
        private static IUnityContainer container;

        public static IUnityContainer Container
        {
            get { return container ?? (container = new UnityContainer()); }
        }
    }
}