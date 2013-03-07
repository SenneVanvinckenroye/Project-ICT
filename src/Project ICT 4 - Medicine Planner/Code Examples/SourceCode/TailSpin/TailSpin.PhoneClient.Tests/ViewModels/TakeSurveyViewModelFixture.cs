//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System.Linq;
using Microsoft.Phone.Scheduler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TailSpin.PhoneClient.ViewModels;
using TailSpin.Phone.TestSupport;
using TailSpin.PhoneClient.Tests.ViewModels.Mocks;
using TailSpin.PhoneServices.Models;

namespace TailSpin.PhoneClient.Tests.ViewModels
{
    [TestClass]
    public class TakeSurveyViewModelFixture
    {
        [TestMethod]
        public void LoadsSurveyFromStringID()
        {
            var store = new SurveyStoreMock();
            var surveyStoreLocator = new SurveyStoreLocatorMock(store);
            store.Initialize();
            store.AllSurveys.Templates.Add(new SurveyTemplate
                                {
                                    Title = "My survey1",
                                    Tenant = "Tenant One",
                                    IconUrl = "http://icon",
                                    Completeness = 100,
                                    IsFavorite = true,
                                    IsNew = true,
                                    Length = 40,
                                    SlugName = "slug1"
                                });

            var allSurveys = store.GetSurveyTemplates();

            var vm = new TakeSurveyViewModel(new MockNavigationService(),
                                             new MockPhoneApplicationServiceFacade(),
                                             new MockLocationService(),
                                             surveyStoreLocator,
                                             new MockShellTile());

            vm.Initialize(allSurveys.First().SlugName);

            Assert.AreEqual(vm.TemplateViewModel.Template.SlugName, allSurveys.First().SlugName);
        }
    }
}
