//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Workers.Surveys
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.Unity;
    using Microsoft.WindowsAzure;
    using TailSpin.Web.Survey.Shared.QueueMessages;
    using Web.Survey.Shared;
    using Web.Survey.Shared.Models;
    using Web.Survey.Shared.Stores;
    using Web.Survey.Shared.Stores.AzureStorage;

    public static class ContainerBootstraper
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            container.RegisterInstance(account);

            container.RegisterType<ISurveyStore, SurveyStore>();

            container.RegisterType<IAzureTable<SurveyRow>, AzureTable<SurveyRow>>(
                new InjectionConstructor(typeof(Microsoft.WindowsAzure.CloudStorageAccount), AzureConstants.Tables.Surveys));

            container.RegisterType<IAzureTable<QuestionRow>, AzureTable<QuestionRow>>(
                new InjectionConstructor(typeof(Microsoft.WindowsAzure.CloudStorageAccount), AzureConstants.Tables.Questions));

            container.RegisterType<IMediaAnswerStore, MediaAnswerStore>(
                new InjectionConstructor(
                    new ResolvedParameter<IAzureBlobContainer<byte[]>>(AzureConstants.BlobContainers.VoiceAnswers),
                    new ResolvedParameter<IAzureBlobContainer<byte[]>>(AzureConstants.BlobContainers.PictureAnswers)));

            container.RegisterType<IAzureBlobContainer<byte[]>, FilesBlobContainer>(
                AzureConstants.BlobContainers.PictureAnswers,
                new InjectionConstructor(
                    typeof(CloudStorageAccount),
                    AzureConstants.BlobContainers.PictureAnswers,
                    "image/jpeg"));

            container.RegisterType<IAzureBlobContainer<byte[]>, FilesBlobContainer>(
                AzureConstants.BlobContainers.VoiceAnswers,
                new InjectionConstructor(
                    typeof(CloudStorageAccount),
                    AzureConstants.BlobContainers.VoiceAnswers,
                    "audio/x-wav"));

            container.RegisterType<ISurveyAnswerStore, SurveyAnswerStore>();

            container.RegisterType<ISurveyAnswersSummaryStore, SurveyAnswersSummaryStore>();

            container.RegisterType<ITenantStore, TenantStore>();

            container.RegisterType<IAzureBlobContainer<byte[]>, FilesBlobContainer>(
                new InjectionConstructor(typeof(Microsoft.WindowsAzure.CloudStorageAccount), AzureConstants.BlobContainers.Logos, "image/jpeg"));

            container.RegisterType<IAzureQueue<SurveyAnswerStoredMessage>, AzureQueue<SurveyAnswerStoredMessage>>(
                new InjectionConstructor(typeof(Microsoft.WindowsAzure.CloudStorageAccount), AzureConstants.Queues.SurveyAnswerStored));

            container.RegisterType<IAzureQueue<NewSurveyMessage>, AzureQueue<NewSurveyMessage>>(
                new InjectionConstructor(typeof(Microsoft.WindowsAzure.CloudStorageAccount), AzureConstants.Queues.NewSurveyStored));

            container.RegisterType<IAzureBlobContainer<SurveyAnswersSummary>, EntitiesBlobContainer<SurveyAnswersSummary>>(
                new InjectionConstructor(typeof(Microsoft.WindowsAzure.CloudStorageAccount), AzureConstants.BlobContainers.SurveyAnswersSummaries));

            container.RegisterType<IAzureBlobContainer<List<string>>, EntitiesBlobContainer<List<string>>>(
                new InjectionConstructor(typeof(Microsoft.WindowsAzure.CloudStorageAccount), AzureConstants.BlobContainers.SurveyAnswersLists));

            container.RegisterType<IDictionary<string, SurveyAnswersSummary>, Dictionary<string, SurveyAnswersSummary>>(
                new InjectionConstructor());

            container.RegisterType<IAzureBlobContainer<Tenant>, EntitiesBlobContainer<Tenant>>(
                new InjectionConstructor(typeof(Microsoft.WindowsAzure.CloudStorageAccount), AzureConstants.BlobContainers.Tenants));

            container.RegisterType<ISurveySqlStore, SurveySqlStore>();

            container.RegisterType<ISurveyTransferStore, SurveyTransferStore>();

            container.RegisterType<IAzureQueue<SurveyTransferMessage>, AzureQueue<SurveyTransferMessage>>(
                new InjectionConstructor(typeof(Microsoft.WindowsAzure.CloudStorageAccount), AzureConstants.Queues.SurveyTransferRequest, TimeSpan.FromMinutes(1)));

            // Container for resolving the survey answer containers
            var surveyAnswerBlobContainerResolver = new UnityContainer();

            surveyAnswerBlobContainerResolver.RegisterInstance(account);

            surveyAnswerBlobContainerResolver.RegisterType<IAzureBlobContainer<SurveyAnswer>, EntitiesBlobContainer<SurveyAnswer>>(
                new InjectionConstructor(typeof(Microsoft.WindowsAzure.CloudStorageAccount), typeof(string)));

            container.RegisterType<ISurveyAnswerContainerFactory, SurveyAnswerContainerFactory>(
                new InjectionConstructor(surveyAnswerBlobContainerResolver));
        }
    }
}