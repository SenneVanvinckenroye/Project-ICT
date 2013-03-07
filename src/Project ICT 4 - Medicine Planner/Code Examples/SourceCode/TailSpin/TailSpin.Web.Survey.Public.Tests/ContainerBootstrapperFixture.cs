//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


//===============================================================================
// Microsoft patterns & practices
// Windows Azure Architecture Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wag.codeplex.com/license)
//===============================================================================


namespace TailSpin.Web.Survey.Public.Tests
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TailSpin.Web.Survey.Public;
    using TailSpin.Web.Survey.Shared.Stores;

    [TestClass]
    public class ContainerBootstrapperFixture
    {
        [TestMethod]
        public void ResolveISurveyStore()
        {
            var container = new UnityContainer();

            ContainerBootstraper.RegisterTypes(container);
            var actualObject = container.Resolve<ISurveyStore>();

            Assert.IsInstanceOfType(actualObject, typeof(SurveyStore));
        }

        [TestMethod]
        public void ResolveISurveyAnswerStore()
        {
            var container = new UnityContainer();

            ContainerBootstraper.RegisterTypes(container);
            var actualObject = container.Resolve<ISurveyAnswerStore>();

            Assert.IsInstanceOfType(actualObject, typeof(SurveyAnswerStore));
        }
    }
}
