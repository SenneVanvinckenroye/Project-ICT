﻿//===============================================================================
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
    using Shared.Stores;
    using Shared.Stores.AzureStorage;
    using TailSpin.Web.Survey.Shared.Models;

    [TestClass]
    public class SurveyAnswerSummaryStoreFixture
    {
        [TestMethod]
        public void SaveSurveyAnswersSummarySavesBlob()
        {
            var mockSurveyAnswersSummaryBlobContainer = new Mock<IAzureBlobContainer<SurveyAnswersSummary>>();
            var store = new SurveyAnswersSummaryStore(mockSurveyAnswersSummaryBlobContainer.Object);
            var surveyAnswersSummary = new SurveyAnswersSummary { Tenant = "tenant", SlugName = "slug-name" };

            store.SaveSurveyAnswersSummary(surveyAnswersSummary);

            mockSurveyAnswersSummaryBlobContainer.Verify(
                c => c.Save("tenant-slug-name", surveyAnswersSummary),
                Times.Once());
        }

        [TestMethod]
        public void GetSurveyAnswersSummaryGetsFromBlobContainer()
        {
            var mockSurveyAnswersSummaryBlobContainer = new Mock<IAzureBlobContainer<SurveyAnswersSummary>>();
            var store = new SurveyAnswersSummaryStore(mockSurveyAnswersSummaryBlobContainer.Object);

            store.GetSurveyAnswersSummary("tenant", "slug-name");

            mockSurveyAnswersSummaryBlobContainer.Verify(
                c => c.Get("tenant-slug-name"),
                Times.Once());
        }

        [TestMethod]
        public void GetSurveyAnswersSummaryReturnsSumamryReadFromBlobContainer()
        {
            var mockSurveyAnswersSummaryBlobContainer = new Mock<IAzureBlobContainer<SurveyAnswersSummary>>();
            var store = new SurveyAnswersSummaryStore(mockSurveyAnswersSummaryBlobContainer.Object);
            var surveyAnswersSummary = new SurveyAnswersSummary();
            mockSurveyAnswersSummaryBlobContainer.Setup(c => c.Get("tenant-slug-name")).Returns(surveyAnswersSummary);

            SurveyAnswersSummary actualSurveyAnswersSummary = store.GetSurveyAnswersSummary("tenant", "slug-name");

            Assert.AreSame(surveyAnswersSummary, actualSurveyAnswersSummary);
        }

        [TestMethod]
        public void DeleteSurveyAnswersSummarySavesBlob()
        {
            var mockSurveyAnswersSummaryBlobContainer = new Mock<IAzureBlobContainer<SurveyAnswersSummary>>();
            var store = new SurveyAnswersSummaryStore(mockSurveyAnswersSummaryBlobContainer.Object);

            store.DeleteSurveyAnswersSummary("tenant", "slug-name");

            mockSurveyAnswersSummaryBlobContainer.Verify(
                c => c.Delete("tenant-slug-name"),
                Times.Once());
        }
    }
}