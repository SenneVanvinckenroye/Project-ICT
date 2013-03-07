//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using Microsoft.Phone.Scheduler;
using TailSpin.PhoneClient.Adapters;
using TailSpin.PhoneServices.Services.Stores;

namespace TailSpin.PhoneClient.Services
{
    public class ScheduledActionClient : IScheduledActionClient
    {
        private readonly ISettingsStore settingsStore;
        private readonly IScheduledActionService scheduledActionService;

        public ScheduledActionClient(ISettingsStore settingsStore, IScheduledActionService scheduledActionService)
        {
            this.settingsStore = settingsStore;
            this.scheduledActionService = scheduledActionService;
        }

        public void AddPeriodicTask(string taskName, string taskDescription)
        {
            AddPeriodicTask(taskName, taskDescription, TimeSpan.Zero);
        }

        public void AddPeriodicTask(string taskName, string taskDescription, TimeSpan debugDelay)
        {
            RemoveTask(taskName);

            var periodicTask = new PeriodicTask(taskName);
            periodicTask.Description = taskDescription;

            scheduledActionService.Add(periodicTask);
#if DEBUG
            if (debugDelay > TimeSpan.Zero)
                scheduledActionService.LaunchForTest(taskName, debugDelay);
#endif
        }

        public void AddResourceIntensiveTask(string taskName, string taskDescription)
        {
            AddResourceIntensiveTask(taskName, taskDescription, TimeSpan.Zero);
        }

        public void AddResourceIntensiveTask(string taskName, string taskDescription, TimeSpan debugDelay)
        {
            RemoveTask(taskName);

            var resourceIntensiveTask = new ResourceIntensiveTask(taskName);
            resourceIntensiveTask.Description = taskDescription;

            scheduledActionService.Add(resourceIntensiveTask);

#if DEBUG
            if (debugDelay > TimeSpan.Zero)
                scheduledActionService.LaunchForTest(taskName, debugDelay);
#endif       
        }

        public void ClearTasks()
        {
            RemoveTask(Constants.PeriodicTaskName);

            //removed only because this sample will normally be reviewed in a debug scenario
            //where the resource-intensive task may run wile the app is in the foreground
            //possibly creating concurrency issues
            RemoveTask(Constants.ResourceTaskName);
        }

        public void EnsureTasks()
        {
            if (UserEnabled)
            {
                try
                {
                    AddPeriodicTask(Constants.PeriodicTaskName, Constants.PeriodicTaskDescription,
                                    TimeSpan.FromMinutes(3));

                    AddResourceIntensiveTask(Constants.ResourceTaskName, Constants.ResourceTaskDescription,
                                             TimeSpan.FromMinutes(3));
                }
                catch
                {
                    //possible exception is hidden here since this method is called
                    //during app closing. Check for OS-level disabling of background tasks
                    //is checked when saving on the Settings page
                }
            }
        }

        public bool IsEnabled
        {
            get
            {
                bool result = true;

                try
                {
                    //currently the only way to check if a user has disabled background agents
                    //at the OS settings level is to attempt to add them
                    AddPeriodicTask(Constants.PeriodicTaskName, Constants.PeriodicTaskDescription);
                    RemoveTask(Constants.PeriodicTaskName);
                }
                catch (InvalidOperationException exception)
                {
                    if (exception.Message.Contains(Constants.DisabledBackgroundException))
                    {
                        result = false;
                    }
                }

                return result;
            }
        }

        public bool UserEnabled
        {
            get
            {
                return !string.IsNullOrEmpty(settingsStore.UserName) && settingsStore.BackgroundTasksAllowed;
            }
        }

        private bool TaskIsScheduled(string taskName)
        {
            var task = scheduledActionService.Find(taskName);

            return task != null && task.IsScheduled;
        }

        public bool TaskIsFound(string taskName)
        {
            var task = scheduledActionService.Find(taskName);

            return task != null;
        }

        public void RemoveTask(string taskName)
        {
            if (TaskIsFound(taskName))
            {
                scheduledActionService.Remove(taskName);
            }
        }
    }
}
