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

namespace TailSpin.PhoneClient.Adapters
{
    public interface IPhoneApplicationServiceFacade
    {
        void Save(string key, object value);
        T Load<T>(string key);
        void Remove(string key);
    }
}