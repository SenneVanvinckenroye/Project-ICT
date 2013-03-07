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
using TailSpin.PhoneClient.Adapters;

namespace TailSpin.Phone.TestSupport
{
    public class MockScheduledActionService : IScheduledActionService
    {
        List<ScheduledAction> ScheduledActions = new List<ScheduledAction>();
 
        public void Add(ScheduledAction action)
        {
            ScheduledActions.Add(action);
        }

        public ScheduledAction Find(string name)
        {
            foreach (var scheduledAction in ScheduledActions)
            {
                if (scheduledAction.Name == name) return scheduledAction;
            }
            return null;
        }

        public IEnumerable<T> GetActions<T>() where T : ScheduledAction
        {
            return null;
        }

        public void LaunchForTest(string name, TimeSpan delay)
        {
            
        }

        public void Remove(string name)
        {
            ScheduledActions.Remove(Find(name));
        }

        public void Replace(ScheduledAction action)
        {
            Remove(action.Name);
            ScheduledActions.Add(action);
        }
    }
}
