//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Web.Survey.Shared.Tests.Stores
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using QueueMessages;
    using Shared.Stores;
    using Shared.Stores.AzureStorage;

    [TestClass]
    public class SurveyTransferStoreFixture
    {
        [TestMethod]
        public void TransferAddsMessageToQueue()
        {
            var mockSurveyTransferQueue = new Mock<IAzureQueue<SurveyTransferMessage>>();
            var store = new SurveyTransferStore(mockSurveyTransferQueue.Object);

            store.Transfer("tenant", "slugName");

            mockSurveyTransferQueue.Verify(q => q.AddMessage(It.Is<SurveyTransferMessage>(m => m.Tenant == "tenant" && m.SlugName == "slugName")));
        }

        [TestMethod]
        public void InitializeEnsuresMessageQueueExists()
        {
            var mockSurveyTransferQueue = new Mock<IAzureQueue<SurveyTransferMessage>>();
            var store = new SurveyTransferStore(mockSurveyTransferQueue.Object);

            store.Initialize();

            mockSurveyTransferQueue.Verify(q => q.EnsureExist());
        }
    }
}