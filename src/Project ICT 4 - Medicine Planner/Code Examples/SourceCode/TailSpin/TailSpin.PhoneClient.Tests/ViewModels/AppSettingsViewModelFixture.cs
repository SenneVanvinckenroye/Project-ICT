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
using TailSpin.Phone.TestSupport;
using TailSpin.PhoneClient.Tests.ViewModels.Mocks;
using TailSpin.PhoneClient.ViewModels;

namespace TailSpin.PhoneClient.Tests.ViewModels
{
    [TestClass]
    [Tag("AppSettingsViewModelFixture")]
    public class AppSettingsViewModelFixture
    {
        [TestMethod]
        public void SettingUserNameAndPasswordDoesNotWriteToStorage()
        {
            var settingsStore = new MockSettingsStore();
            var appSettingsViewModel = new AppSettingsViewModel(settingsStore,
                                                                new MockRegistrationService(),
                                                                new MockNavigationService(),
                                                                new MockPhoneApplicationServiceFacade(),
                                                                new MockScheduledActionClient(),
                                                                new MockMessageBox());

            appSettingsViewModel.UserName = "username";
            appSettingsViewModel.Password = "password";

            Assert.AreEqual(null, settingsStore.UserName);
            Assert.AreEqual(null, settingsStore.Password);
        }

        [TestMethod]
        public void ExecutingSubmitCommandCommitsUserNameAndPasswordToStorage()
        {
            var settingsStore = new MockSettingsStore();
            var appSettingsViewModel = new AppSettingsViewModel(settingsStore,
                                                                new MockRegistrationService(),
                                                                new MockNavigationService(),
                                                                new MockPhoneApplicationServiceFacade(), 
                                                                new MockScheduledActionClient(),
                                                                new MockMessageBox());
            appSettingsViewModel.UserName = "username";
            appSettingsViewModel.Password = "password";

            appSettingsViewModel.Submit();

            Assert.AreEqual("username", settingsStore.UserName);
            Assert.AreEqual("password", settingsStore.Password);
        }

        [TestMethod]
        public void WhenCreatedViewModelLoadsFromStore()
        {
            var settingsStore = new MockSettingsStore();
            settingsStore.UserName = "username";
            settingsStore.Password = "password";

            var appSettingsViewModel = new AppSettingsViewModel(settingsStore,
                                                                new MockRegistrationService(),
                                                                new MockNavigationService(),
                                                                new MockPhoneApplicationServiceFacade(), 
                                                                new MockScheduledActionClient(),
                                                                new MockMessageBox());

            Assert.AreEqual("username", appSettingsViewModel.UserName);
            Assert.AreEqual("password", appSettingsViewModel.Password);
        }

        [TestMethod]
        public void IfUserNameNotEnteredThenCanSubmitFalse()
        {
            var settingsStore = new MockSettingsStore();
            var appSettingsViewModel = new AppSettingsViewModel(settingsStore,
                                                                new MockRegistrationService(),
                                                                new MockNavigationService(),
                                                                new MockPhoneApplicationServiceFacade(), 
                                                                new MockScheduledActionClient(),
                                                                new MockMessageBox());

            Assert.IsFalse(appSettingsViewModel.CanSubmit);
        }

        [TestMethod]
        public void IfUserNameEnteredThenCanSubmitFalse()
        {
            var settingsStore = new MockSettingsStore();
            var appSettingsViewModel = new AppSettingsViewModel(settingsStore,
                                                                new MockRegistrationService(),
                                                                new MockNavigationService(),
                                                                new MockPhoneApplicationServiceFacade(), 
                                                                new MockScheduledActionClient(),
                                                                new MockMessageBox());

            appSettingsViewModel.UserName = "testUser";

            Assert.IsFalse(appSettingsViewModel.CanSubmit);
        }

        [TestMethod]
        public void IfUserNameAndPasswordEnteredThenCanSubmit()
        {
            var settingsStore = new MockSettingsStore();
            var appSettingsViewModel = new AppSettingsViewModel(settingsStore,
                                                                new MockRegistrationService(),
                                                                new MockNavigationService(),
                                                                new MockPhoneApplicationServiceFacade(), 
                                                                new MockScheduledActionClient(),
                                                                new MockMessageBox());

            appSettingsViewModel.UserName = "testUser";
            appSettingsViewModel.Password = "testPassword";

            Assert.IsTrue(appSettingsViewModel.CanSubmit);
        }
    }
}
