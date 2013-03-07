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
    using System.Collections.Generic;

    public interface IAzureQueue<T> where T : AzureQueueMessage
    {
        void EnsureExist();
        void Clear();
        void AddMessage(T message);
        T GetMessage();
        IEnumerable<T> GetMessages(int maxMessagesToReturn);
        void DeleteMessage(T message);
    }
}