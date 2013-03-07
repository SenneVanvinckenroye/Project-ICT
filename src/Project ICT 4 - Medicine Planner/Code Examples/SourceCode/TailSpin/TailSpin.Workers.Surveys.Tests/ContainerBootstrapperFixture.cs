//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Workers.Surveys.Tests
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TailSpin.Workers.Surveys;
    using Web.Survey.Shared.Stores;

    [TestClass]
    public class ContainerBootstrapperFixture
    {
        [TestMethod]
        public void ResolveISurveyAnswerStore()
        {
            var container = new UnityContainer();

            ContainerBootstraper.RegisterTypes(container);
            var actualObject = container.Resolve<ISurveyAnswerStore>();

            Assert.IsInstanceOfType(actualObject, typeof(SurveyAnswerStore));
        }

        [TestMethod]
        public void ResolveISurveyAnswersSummaryStore()
        {
            var container = new UnityContainer();

            ContainerBootstraper.RegisterTypes(container);
            var actualObject = container.Resolve<ISurveyAnswersSummaryStore>();

            Assert.IsInstanceOfType(actualObject, typeof(SurveyAnswersSummaryStore));
        }
    }
}
