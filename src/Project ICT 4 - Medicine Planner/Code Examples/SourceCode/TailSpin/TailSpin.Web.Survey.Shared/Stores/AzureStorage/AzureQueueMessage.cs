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
    public abstract class AzureQueueMessage
    {
        public string Id { get; set; }
        public string PopReceipt { get; set; }
        public int DequeueCount { get; set; }
    }
}