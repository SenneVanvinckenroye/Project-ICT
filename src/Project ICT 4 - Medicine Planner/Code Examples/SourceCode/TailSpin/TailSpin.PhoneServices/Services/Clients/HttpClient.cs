//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using Microsoft.Phone.Reactive;
using TailSpin.Phone.Adapters;

namespace TailSpin.PhoneServices.Services.Clients
{
    public class HttpClient : IHttpClient
    {
        public IHttpWebRequest GetRequest(IHttpWebRequest httpWebRequest, string userName, string password)
        {
            var authHeader = string.Format(CultureInfo.InvariantCulture, "user {0}:{1}", userName, password);
            httpWebRequest.Headers[HttpRequestHeader.Authorization] = authHeader;
            return httpWebRequest;
        }

        public IObservable<T> GetJson<T>(IHttpWebRequest httpWebRequest, string userName, string password)
        {
            var request = GetRequest(httpWebRequest, userName, password);
            request.Method = "GET";
            request.Accept = "application/json";

            return
                Observable
                    .FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)()
                    .Select(
                        response =>
                        {
                            using (var responseStream = response.GetResponseStream())
                            {
                                var serializer = new DataContractJsonSerializer(typeof(T));
                                return (T)serializer.ReadObject(responseStream);
                            }
                        });
        }

        public IObservable<Unit> PostJson<T>(IHttpWebRequest httpWebRequest, string userName, string password, T obj)
        {
            var request = GetRequest(httpWebRequest, userName, password);
            request.Method = "POST";
            request.ContentType = "application/json";

            return
                from requestStream in
                    Observable
                    .FromAsyncPattern<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream)()
                from response in WriteContentToStream(requestStream, request, obj)
                select new Unit();
        }

        private IObservable<WebResponse> WriteContentToStream<T>(Stream requestStream, IHttpWebRequest request, T obj)
        {
            using (requestStream)
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(requestStream, obj);
            }

            return
                Observable.FromAsyncPattern<WebResponse>(
                    request.BeginGetResponse,
                    request.EndGetResponse)();
        }
    }
}
