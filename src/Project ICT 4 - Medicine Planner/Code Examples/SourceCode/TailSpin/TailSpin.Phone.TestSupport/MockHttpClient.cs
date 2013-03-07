//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using Microsoft.Phone.Reactive;
using TailSpin.Phone.Adapters;
using TailSpin.PhoneServices.Services.Clients;

namespace TailSpin.Phone.TestSupport
{
    public class MockHttpClient : IHttpClient
    {
        public object GetJsonReturnValue { get; set; }
        public Phone.Adapters.IHttpWebRequest GetRequest(IHttpWebRequest httpWebRequest, string userName, string password)
        {
            throw new NotImplementedException();
        }

        public IObservable<T> GetJson<T>(IHttpWebRequest httpWebRequest, string userName, string password)
        {
            return (Observable.Return((T)GetJsonReturnValue));
        }

        public IObservable<Unit> PostJson<T>(IHttpWebRequest httpWebRequest, string userName, string password, T obj)
        {
            throw new NotImplementedException();
        }
    }
}
