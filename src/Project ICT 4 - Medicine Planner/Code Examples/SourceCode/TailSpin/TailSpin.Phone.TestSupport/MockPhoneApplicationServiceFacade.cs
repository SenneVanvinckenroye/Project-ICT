//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using Microsoft.Phone.Shell;
using TailSpin.PhoneClient.Adapters;

namespace TailSpin.Phone.TestSupport
{
    public class MockPhoneApplicationServiceFacade : IPhoneApplicationServiceFacade
    {
        public MockPhoneApplicationServiceFacade()
        {
            this.LoadTestCallback = (key) => null;
        }

        public Action<string, object> SaveTestCallback { get; set; }
        public Func<string, object> LoadTestCallback { get; set; }
        public Action<string> RemoveTestCallback { get; set; }

        public void Save(string key, object value)
        {
            if (SaveTestCallback != null) SaveTestCallback(key, value);
        }

        public T Load<T>(string key)
        {
            if (LoadTestCallback(key) == null) return default(T);
            return (T)LoadTestCallback(key);
        }

        public void Remove(string key)
        {
            RemoveTestCallback(key);
        }
    }
}
