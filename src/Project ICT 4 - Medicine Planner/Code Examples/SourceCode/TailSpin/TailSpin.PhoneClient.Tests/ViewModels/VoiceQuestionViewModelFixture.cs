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
using TailSpin.PhoneClient.ViewModels;
using TailSpin.PhoneServices.Models;

namespace TailSpin.PhoneClient.Tests.ViewModels
{
    [TestClass]
    [Tag("VoiceQuestionViewModelFixture")]
    public class VoiceQuestionViewModelFixture
    {
        [TestMethod]
        public void WhenCreatedIsRecordingIsFalse()
        {
            var answer = new QuestionAnswer();
            var vm = new VoiceQuestionViewModel(answer, new MockIsolatedStorageFacade(), new MockNavigationService());

            Assert.IsFalse(vm.IsRecording);
        }

        [TestMethod]
        public void WhenCreatedActionTextIsStartRecording()
        {
            var expected = "Start Recording";
            var answer = new QuestionAnswer();
            var vm = new VoiceQuestionViewModel(answer, new MockIsolatedStorageFacade(), new MockNavigationService());

            Assert.AreEqual(expected, vm.DefaultActionText);
        }

        [TestMethod]
        public void WhenInvokeDefaultAndNotRecordingRecordingComesTrue()
        {
            var answer = new QuestionAnswer();
            var vm = new VoiceQuestionViewModel(answer, new MockIsolatedStorageFacade(), new MockNavigationService());
            var initialState = vm.IsRecording;
            vm.DefaultActionCommand.Execute(null);
            Assert.IsFalse(initialState);
            Assert.IsTrue(vm.IsRecording);
        }

        [TestMethod]
        public void WhenInvokeDefaultAndNotRecordingActionTextComesStopRecording()
        {
            var expected = "Stop Recording";
            var answer = new QuestionAnswer();
            var vm = new VoiceQuestionViewModel(answer, new MockIsolatedStorageFacade(), new MockNavigationService());
            var initialState = vm.IsRecording;
            vm.DefaultActionCommand.Execute(null);
            Assert.IsFalse(initialState);
            Assert.AreEqual(expected, vm.DefaultActionText);
        }

        [TestMethod]
        public void WhenInvokeDefaultAndRecordingRecordingComesTrue()
        {
            var answer = new QuestionAnswer();
            var vm = new VoiceQuestionViewModel(answer, new MockIsolatedStorageFacade(), new MockNavigationService());
            vm.DefaultActionCommand.Execute(null);
            var initialState = vm.IsRecording;
            vm.DefaultActionCommand.Execute(null);
            Assert.IsTrue(initialState);
            Assert.IsFalse(vm.IsRecording);
        }

        [TestMethod]
        public void WhenInvokeDefaultAndRecordingActionTextComesStartRecording()
        {
            var expected = "Start Recording";
            var answer = new QuestionAnswer();
            var vm = new VoiceQuestionViewModel(answer, new MockIsolatedStorageFacade(), new MockNavigationService());
            vm.DefaultActionCommand.Execute(null);
            var initialState = vm.IsRecording;
            vm.DefaultActionCommand.Execute(null);
            Assert.IsTrue(initialState);
            Assert.AreEqual(expected, vm.DefaultActionText);
        }

        [TestMethod]
        public void WhenInvokeDefaultAndFrameObscuredRecordingStops()
        {
            var answer = new QuestionAnswer();
            var navigationService = new MockNavigationService();
            var vm = new VoiceQuestionViewModel(answer, new MockIsolatedStorageFacade(), navigationService);
            vm.DefaultActionCommand.Execute(null);
            var initialState = vm.IsRecording;
            navigationService.RaiseObscured();
            Assert.IsTrue(initialState);
            Assert.IsFalse(vm.IsRecording);
        }
    }
}
