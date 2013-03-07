//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Web.Survey.Shared.QueueMessages
{
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;

    public class SurveyTransferMessage : AzureQueueMessage
    {
        public string Tenant { get; set; }
        public string SlugName { get; set; }
    }
}