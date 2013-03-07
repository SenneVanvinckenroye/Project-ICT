//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Phone.Scheduler;

namespace TailSpin.PhoneClient.Adapters
{
    public class ScheduledActionServiceAdapter : IScheduledActionService
    {
        public void Add(ScheduledAction action)
        {
            ScheduledActionService.Add(action);
        }

        public ScheduledAction Find(string name)
        {
            return ScheduledActionService.Find(name);
        }

        public IEnumerable<T> GetActions<T>() where T : ScheduledAction
        {
            return ScheduledActionService.GetActions<T>();
        }

        public void LaunchForTest(string name, TimeSpan delay)
        {
            ScheduledActionService.LaunchForTest(name, delay);
        }

        public void Remove(string name)
        {
            ScheduledActionService.Remove(name);
        }

        public void Replace(ScheduledAction action)
        {
            ScheduledActionService.Replace(action);
        }
    }
}
