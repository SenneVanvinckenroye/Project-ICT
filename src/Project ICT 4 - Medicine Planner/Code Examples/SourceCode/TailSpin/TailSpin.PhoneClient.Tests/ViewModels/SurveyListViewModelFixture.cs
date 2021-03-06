﻿//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System.Collections.Generic;
using System.Linq;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TailSpin.Phone.TestSupport;
using TailSpin.PhoneClient.Tests.ViewModels.Mocks;
using TailSpin.PhoneClient.ViewModels;
using TailSpin.PhoneServices.Models;
using TailSpin.PhoneServices.Services.Stores;

namespace TailSpin.PhoneClient.Tests.ViewModels
{
    [TestClass]
    [Tag("SurveyListViewModelFixture")]
    public class SurveyListViewModelFixture
    {
        [TestMethod]
        public void NewsSectionShowsNewItems()
        {
            var store = new SurveyStoreMock();
            var surveyStoreLocator = new SurveyStoreLocatorMock(store);
            store.Initialize();
            var allSurveys = store.GetSurveyTemplates();

            var vm = new SurveyListViewModel(surveyStoreLocator,
                                             new SurveysSynchronizationServiceMock(),
                                             new MockNavigationService(),
                                             new MockPhoneApplicationServiceFacade(), 
                                             new MockShellTile(),
                                             new MockSettingsStore(),
                                             new MockLocationService());
            vm.Refresh();
            
            var newSurveys = vm.NewItems.Cast<SurveyTemplateViewModel>().ToList();
            CollectionAssert.AreEquivalent(allSurveys.Where(p => p.IsNew).ToArray(), newSurveys.Select(t => t.Template).ToArray());
        }

        [TestMethod]
        public void FavoritesSectionShowsFavoritedItems()
        {
            var store = new SurveyStoreMock();
            var surveyStoreLocator = new SurveyStoreLocatorMock(store);
            store.Initialize();
            var allSurveys = store.GetSurveyTemplates();

            var vm = new SurveyListViewModel(surveyStoreLocator,
                                             new SurveysSynchronizationServiceMock(),
                                             new MockNavigationService(),
                                             new MockPhoneApplicationServiceFacade(), 
                                             new MockShellTile(),
                                             new MockSettingsStore(),
                                             new MockLocationService());
            vm.Refresh();
            
            var favoriteSurveys = vm.FavoriteItems.Cast<SurveyTemplateViewModel>().ToList();
            CollectionAssert.AreEquivalent(allSurveys.Where(p => p.IsFavorite).ToArray(), favoriteSurveys.Select(t => t.Template).ToArray());
        }

        [TestMethod]
        public void ByLengthSectionShowsItemsOrderedByLengthAscending()
        {
            var store = new SurveyStoreMock();
            var surveyStoreLocator = new SurveyStoreLocatorMock(store);
            store.Initialize();
            var allSurveys = store.GetSurveyTemplates();

            var vm = new SurveyListViewModel(surveyStoreLocator,
                                             new SurveysSynchronizationServiceMock(),
                                             new MockNavigationService(),
                                             new MockPhoneApplicationServiceFacade(), 
                                             new MockShellTile(),
                                             new MockSettingsStore(),
                                             new MockLocationService());
            vm.Refresh();
            
            var surveysByLength = vm.ByLengthItems.Cast<SurveyTemplateViewModel>().ToList();
            CollectionAssert.AreEquivalent(allSurveys.OrderBy(p => p.Length).ToArray(), surveysByLength.Select(t => t.Template).ToArray());
        }

        [TestMethod]
        public void EmptyStoreCreatesEmptySections()
        {
            var store = new SurveyStoreMock(new List<SurveyTemplate>());
            var surveyStoreLocator = new SurveyStoreLocatorMock(store);
            store.Initialize();

            var vm = new SurveyListViewModel(surveyStoreLocator,
                                             new SurveysSynchronizationServiceMock(),
                                             new MockNavigationService(),
                                             new MockPhoneApplicationServiceFacade(), 
                                             new MockShellTile(),
                                             new MockSettingsStore(),
                                             new MockLocationService());
            vm.Refresh();

            Assert.AreEqual(0, vm.NewItems.Cast<SurveyTemplateViewModel>().Count());
            Assert.AreEqual(0, vm.FavoriteItems.Cast<SurveyTemplateViewModel>().Count());
            Assert.AreEqual(0, vm.ByLengthItems.Cast<SurveyTemplateViewModel>().Count());
        }

        [TestMethod]
        public void ViewModelIsCreatedWithIsSyncrhonizingFalse()
        {
            var store = new SurveyStoreMock(new List<SurveyTemplate>());
            var surveyStoreLocator = new SurveyStoreLocatorMock(store);
            store.Initialize();

            var vm = new SurveyListViewModel(surveyStoreLocator,
                                             new SurveysSynchronizationServiceMock(),
                                             new MockNavigationService(),
                                             new MockPhoneApplicationServiceFacade(), 
                                             new MockShellTile(),
                                             new MockSettingsStore(),
                                             new MockLocationService());
            
            Assert.IsFalse(vm.IsSynchronizing);
        }
    }
}
