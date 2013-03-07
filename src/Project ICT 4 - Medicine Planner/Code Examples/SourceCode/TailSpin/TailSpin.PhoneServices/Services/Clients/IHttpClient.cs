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

namespace TailSpin.PhoneServices.Services.Clients
{
    public interface IHttpClient
    {
        IHttpWebRequest GetRequest(IHttpWebRequest httpWebRequest, string userName, string password);
        IObservable<T> GetJson<T>(IHttpWebRequest httpWebRequest, string userName, string password);
        IObservable<Unit> PostJson<T>(IHttpWebRequest httpWebRequest, string userName, string password, T obj);
    }
}
