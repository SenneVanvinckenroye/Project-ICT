//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Web.Survey.Shared.Stores.AzureStorage
{
    using System;
    using System.Collections.Generic;

    public interface IAzureBlobContainer<T>
    {
        void EnsureExist();
        void Save(string objId, T obj);
        T Get(string objId);
        IEnumerable<T> GetAll();
        Uri GetUri(string objId);
        void Delete(string objId);
        void DeleteContainer();
    }
}