//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using Microsoft.Phone.Scheduler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TailSpin.Phone.TestSupport;
using TailSpin.PhoneClient.Services;
using TailSpin.PhoneClient.Tests.ViewModels.Mocks;
using TailSpin.PhoneServices.Services;

namespace TailSpin.PhoneClient.Tests.Services
{
    [TestClass]
    public class ScheduledActionClientFixture
    {
        [TestMethod]
        public void AddNewPeriodicTask()
        {
            var testTaskName = "TestTaskName";
            var settingsStore = new MockSettingsStore();
            var scheduledActionService = new MockScheduledActionService();
            settingsStore.UserName = "fred";
            var target = new ScheduledActionClient(settingsStore, scheduledActionService);

            target.AddPeriodicTask(testTaskName, "TestTaskDescription");
            var task = scheduledActionService.Find(testTaskName);

            Assert.IsNotNull(task);
            Assert.IsInstanceOfType(task, typeof(PeriodicTask));
        }

        [TestMethod]
        public void AddNewResourceIntensiveTask()
        {
            var testTaskName = "TestTaskName";
            var settingsStore = new MockSettingsStore();
            var scheduledActionService = new MockScheduledActionService();
            settingsStore.UserName = "fred";
            var target = new ScheduledActionClient(settingsStore, scheduledActionService);

            target.AddResourceIntensiveTask(testTaskName, "TestTaskDescription");
            var task = scheduledActionService.Find(testTaskName);

            Assert.IsNotNull(task);
            Assert.IsInstanceOfType(task, typeof(ResourceIntensiveTask));
        }

        [TestMethod]
        public void IfTaskNotFoundIsRunningFalse()
        {
            var testTaskName = "TestTaskName";
            var settingsStore = new MockSettingsStore();
            var scheduledActionService = new MockScheduledActionService();
            settingsStore.UserName = "fred";
            var target = new ScheduledActionClient(settingsStore, scheduledActionService);

            var result = target.TaskIsFound(testTaskName);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RemoveTask()
        {
            var testTaskName = "TestTaskName";
            var settingsStore = new MockSettingsStore();
            var scheduledActionService = new MockScheduledActionService();
            settingsStore.UserName = "fred";
            var target = new ScheduledActionClient(settingsStore, scheduledActionService);

            target.AddPeriodicTask(testTaskName, "TestTaskDescription");
            var task = scheduledActionService.Find(testTaskName);
            Assert.IsNotNull(task);

            target.RemoveTask(testTaskName);
            task = scheduledActionService.Find(testTaskName);
            Assert.IsNull(task);
        }
    }
}
