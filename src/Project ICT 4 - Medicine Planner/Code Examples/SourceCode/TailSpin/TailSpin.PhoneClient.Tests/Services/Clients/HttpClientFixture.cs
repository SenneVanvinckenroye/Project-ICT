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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TailSpin.Phone.Adapters;
using TailSpin.PhoneServices.Services.Clients;

namespace TailSpin.PhoneClient.Tests.Services.Clients
{
    [TestClass]
    public class HttpClientFixture
    {
        [TestMethod]
        public void ClientSetsAuthorizationHeaderOnRequest()
        {
            const string userName = "Joe";
            const string password = "password";
            var authHeader = string.Format(CultureInfo.InvariantCulture, "user {0}:{1}", userName, password);
            var target = new HttpClient();
            var request = target.GetRequest(new MockHttpWebRequestAdapter(), userName, password);

            Assert.AreEqual(authHeader, request.Headers[HttpRequestHeader.Authorization]);
        }
    }

    public class MockHttpWebRequestAdapter : IHttpWebRequest
    {
        public MockHttpWebRequestAdapter()
        {
            Headers = new WebHeaderCollection();
        }

        public bool AllowReadStreamBuffering
        {
            get;
            set;
        }

        public CookieContainer CookieContainer
        {
            get;
            set;
        }

        public Uri RequestUri
        {
            get;
            set;
        }

        public bool AllowAutoRedirect
        {
            get;
            set;
        }

        public bool HaveResponse
        {
            get;
            set;
        }

        public string Method
        {
            get;
            set;
        }

        public ICredentials Credentials
        {
            get;
            set;
        }

        public WebHeaderCollection Headers
        {
            get;
            set;
        }

        public string ContentType
        {
            get;
            set;
        }

        public string Accept
        {
            get;
            set;
        }

        public string UserAgent
        {
            get;
            set;
        }

        public bool SupportsCookieContainer
        {
            get;
            set;
        }

        public bool UseDefaultCredentials
        {
            get;
            set;
        }

        public IWebRequestCreate CreatorInstance
        {
            get;
            set;
        }

        public void Abort()
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public Stream EndGetRequestStream(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public WebResponse EndGetResponse(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }

        public bool RegisterPrefix(string prefix, IWebRequestCreate creator)
        {
            throw new NotImplementedException();
        }

        public HttpWebRequest CreateHttp(Uri requestUri)
        {
            throw new NotImplementedException();
        }

        public HttpWebRequest CreateHttp(string requestUriString)
        {
            throw new NotImplementedException();
        }

        public WebRequest Create(string requestUriString)
        {
            throw new NotImplementedException();
        }

        public WebRequest Create(Uri requestUri)
        {
            throw new NotImplementedException();
        }
    }
}
