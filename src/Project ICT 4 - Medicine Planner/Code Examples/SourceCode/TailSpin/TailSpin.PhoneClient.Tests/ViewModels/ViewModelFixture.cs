//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using System.Collections.Generic;
using System.Windows.Navigation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TailSpin.Phone.TestSupport;
using TailSpin.PhoneClient.Adapters;
using TailSpin.PhoneClient.ViewModels;

namespace TailSpin.PhoneClient.Tests.ViewModels
{
    [TestClass]
    public class ViewModelFixture
    {
        [TestMethod]
        public void NavigatingToViewModelWithoutTombstoningDoesNotCallReactivation()
        {
            var navigationService = new MockNavigationService
                                        {
                                            RecoveredFromTombstoning = true,
                                            DoesPageNeedToRecoverFromTombstoningTestCallback = 
                                            (uri)=>
                                                    {
                                                        if (uri == new Uri("http://test1.org")) return true;
                                                        return false;
                                                    }
                                        };
            var vm1 = new TestableModel(navigationService, new Uri("http://test1.org"));
            var vm2 = new TestableModel(navigationService, new Uri("http://test2.org"));
            navigationService.Navigate(new Uri("http://test1.org"));

            Assert.AreEqual("tombstoned value", vm1.Property1);

            //Mimic page to page navigation.
            navigationService.Navigate(new Uri("http://test2.org"));

            Assert.AreEqual(null, vm2.Property1);
        }

        [TestMethod]
        public void NavigatingAwayFromCurrentViewModelCallsItsNavigatedFrom()
        {
            var navigationService = new MockNavigationService();
            var vm1 = new TestableModel(navigationService, new Uri("http://test1.org"));
            var vm2 = new TestableModel(navigationService, new Uri("http://test2.org"));
            navigationService.Navigate(new Uri("http://test1.org"));

            Assert.IsFalse(vm1.OnPageDeactivationCalled);

            navigationService.Navigate(new Uri("http://otheruri.org"));

            Assert.IsTrue(vm1.OnPageDeactivationCalled);
            Assert.IsFalse(vm2.OnPageDeactivationCalled);
        }

        [TestMethod]
        public void IntentionalNavigationFromViewModelCallsPageDeactivationWithProperFlag()
        {
            var navigationService = new MockNavigationService();
            var vm = new TestableModel(navigationService, new Uri("http://test.org"));
            navigationService.Navigate(new Uri("http://test.org"));

            navigationService.IsNavigationInitiator = true;
            navigationService.Navigate(new Uri("http://otheruri.org"));

            Assert.IsTrue(vm.OnPageDeactivationCalled);
            Assert.IsTrue(vm.IsIntentionalNavigationParam);
        }

        [TestMethod]
        public void UnintentionalNavigationFromViewModelCallsPageDeactivationWithProperFlag()
        {
            var navigationService = new MockNavigationService();
            var vm = new TestableModel(navigationService, new Uri("http://test.org"));
            navigationService.Navigate(new Uri("http://test.org"));

            navigationService.IsNavigationInitiator = false;
            navigationService.Navigate(new Uri("http://otheruri.org"));

            Assert.IsTrue(vm.OnPageDeactivationCalled);
            Assert.IsFalse(vm.IsIntentionalNavigationParam);
        }
    }

    public class TestableModel : ViewModel
    {
        public bool OnPageDeactivationCalled { get; set; }
        public bool IsIntentionalNavigationParam { get; set; }
        public string Property1 { get; set; }
        public TestableModel(INavigationService navigationService, Uri uri) : base(navigationService, new MockPhoneApplicationServiceFacade(), uri)
        {
            
        }

        public override void OnPageResumeFromTombstoning()
        {
            Property1 = "tombstoned value";
        }

        public override void OnPageDeactivation(bool isIntentionalNavigation)
        {
            OnPageDeactivationCalled = true;
            IsIntentionalNavigationParam = isIntentionalNavigation;
        }
    }
}
