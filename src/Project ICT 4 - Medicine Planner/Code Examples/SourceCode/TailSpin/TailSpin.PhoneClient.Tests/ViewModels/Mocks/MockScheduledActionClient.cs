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
using Microsoft.Phone.Scheduler;
using TailSpin.PhoneClient.Services;

namespace TailSpin.PhoneClient.Tests.ViewModels.Mocks
{
    public class MockScheduledActionClient :IScheduledActionClient
    {
        public List<PeriodicTask> PeriodicTasks = new List<PeriodicTask>();
        public List<ResourceIntensiveTask> ResourceIntensiveTasks = new List<ResourceIntensiveTask>();

        public void AddPeriodicTask(string taskName, string taskDescription)
        {
            var periodicTask = new PeriodicTask(taskName);
            periodicTask.Description = taskDescription;

            PeriodicTasks.Add(periodicTask);
        }

        public void AddPeriodicTask(string taskName, string taskDescription, TimeSpan debugDelay)
        {
            AddPeriodicTask(taskName, taskDescription);
        }

        public void AddResourceIntensiveTask(string taskName, string taskDescription)
        {
            var resourceIntensiveTask = new ResourceIntensiveTask(taskName);
            resourceIntensiveTask.Description = taskDescription;

            ResourceIntensiveTasks.Add(resourceIntensiveTask);
        }

        public void AddResourceIntensiveTask(string taskName, string taskDescription, TimeSpan debugDelay)
        {
            AddResourceIntensiveTask(taskName, taskDescription);
        }

        public void ClearTasks()
        {
        }

        public void EnsureTasks()
        {
        }

        public bool IsEnabled
        {
            get
            {
                return true;
            }
        }

        public bool UserEnabled
        {
            get
            {
                return true;
            }
        }

        public bool TaskIsFound(string name)
        {
            return FindPeriodicTask(name) != null || FindResourceIntensiveTask(name) != null;
        }

        public bool TaskIsScheduled(string name)
        {
            return FindPeriodicTask(name) != null || FindResourceIntensiveTask(name) != null;
        }

        public void RemoveTask(string name)
        {
            var periodicTask = FindPeriodicTask(name);
            if (periodicTask != null)
            {
                PeriodicTasks.Remove(periodicTask);
            }

            var resourceIntensiveTask = FindResourceIntensiveTask(name);
            if (resourceIntensiveTask != null)
            {
                ResourceIntensiveTasks.Remove(resourceIntensiveTask);
            }
        }

        public PeriodicTask FindPeriodicTask(string name)
        {
            foreach (var periodicTask in PeriodicTasks)
            {
                if (periodicTask.Name == name) return periodicTask;
            }
            return null;
        }

        public ResourceIntensiveTask FindResourceIntensiveTask(string name)
        {
            foreach (var resourceIntensiveTask in ResourceIntensiveTasks)
            {
                if (resourceIntensiveTask.Name == name) return resourceIntensiveTask;
            }
            return null;
        }
    }
}
