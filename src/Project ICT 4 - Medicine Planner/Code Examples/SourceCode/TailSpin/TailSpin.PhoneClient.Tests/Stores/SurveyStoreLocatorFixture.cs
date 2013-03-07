//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TailSpin.PhoneClient.Tests.ViewModels.Mocks;
using TailSpin.PhoneServices.Services.Stores;

namespace TailSpin.PhoneClient.Tests.Stores
{
    [Tag("Store")]
    [TestClass]
    public class SurveyStoreLocatorFixture
    {
        [TestMethod]
        public void ReturnEmptySurveyStoreWhenUsernameIsEmpty()
        {
            ISettingsStore settingsStore = new MockSettingsStore();
            ISurveyStoreLocator surveyStoreLocator = 
                new SurveyStoreLocator(settingsStore, storeName => new SurveyStoreMock());

            settingsStore.UserName = string.Empty;
            var surveyStore = surveyStoreLocator.GetStore();

            Assert.IsInstanceOfType(surveyStore, typeof(NullSurveyStore));
        }

        [TestMethod]
        public void ReturnSameStoreWhenUsernameDidNotChange()
        {
            ISettingsStore settingsStore = new MockSettingsStore();
            ISurveyStoreLocator surveyStoreLocator = 
                new SurveyStoreLocator(settingsStore, storeName => new SurveyStoreMock());

            settingsStore.UserName = "username";
            var surveyStore = surveyStoreLocator.GetStore();
            var storeWithoutSettingsChanged = surveyStoreLocator.GetStore();

            Assert.AreSame(surveyStore, storeWithoutSettingsChanged);
        }

        [TestMethod]
        public void ReturnNewStoreWhenUsernameChanged()
        {
            ISettingsStore settingsStore = new MockSettingsStore();
            ISurveyStoreLocator surveyStoreLocator = 
                new SurveyStoreLocator(settingsStore, storeName => new SurveyStoreMock());

            settingsStore.UserName = "username";
            var storeBeforeSettingsChanged = surveyStoreLocator.GetStore();
            settingsStore.UserName = "username changed";
            var storeAfterSettingsChanged = surveyStoreLocator.GetStore();

            Assert.AreNotSame(storeBeforeSettingsChanged, storeAfterSettingsChanged);
        }
    }
}
