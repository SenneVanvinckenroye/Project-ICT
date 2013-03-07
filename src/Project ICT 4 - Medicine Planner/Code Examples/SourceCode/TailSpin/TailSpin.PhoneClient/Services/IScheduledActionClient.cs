//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;

namespace TailSpin.PhoneClient.Services
{
    public interface IScheduledActionClient
    {
        void AddPeriodicTask(string taskName, string taskDescription);
        void AddPeriodicTask(string taskName, string taskDescription, TimeSpan debugDelay);
        void AddResourceIntensiveTask(string taskName, string taskDescription);
        void AddResourceIntensiveTask(string taskName, string taskDescription, TimeSpan debugDelay);
        void ClearTasks();
        void EnsureTasks();
        bool IsEnabled { get; }
        void RemoveTask(string taskName);
        bool TaskIsFound(string taskName);
        bool UserEnabled { get; }
    }
}