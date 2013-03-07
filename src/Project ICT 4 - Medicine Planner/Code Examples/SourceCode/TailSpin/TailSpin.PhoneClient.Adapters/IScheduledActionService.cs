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

namespace TailSpin.PhoneClient.Adapters
{
    public interface IScheduledActionService
    {
        void Add(ScheduledAction action);
        ScheduledAction Find(string name);
        IEnumerable<T> GetActions<T>() where T : ScheduledAction;
        void LaunchForTest(string name, TimeSpan delay);
        void Remove(string name);
        void Replace(ScheduledAction action);
    }
}
