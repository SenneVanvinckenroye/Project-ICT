//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Web.Survey.Shared
{
    using System.Collections.Generic;
    using Microsoft.Practices.Unity;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.StorageClient;
    using TailSpin.Web.Survey.Shared.Models;
    using TailSpin.Web.Survey.Shared.Notifications;
    using TailSpin.Web.Survey.Shared.QueueMessages;
    using TailSpin.Web.Survey.Shared.Stores;
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;
    using TailSpin.Web.Survey.Shared.SurveysFiltering;

    public static class ComponentRegistration
    {
        public static void InitializeSurveyAnswerStore(IUnityContainer container)
        {
            RegisterSurveyAnswerStore(container);

            container.Resolve<IMediaAnswerStore>().Initialize();
            container.Resolve<ISurveyAnswerStore>().Initialize();
        }

        public static void InitializeSurveyAnswerTransferStore(IUnityContainer container)
        {
            RegisterSurveyAnswerTransferStore(container);

            container.Resolve<ISurveyTransferStore>().Initialize();
        }

        public static void InitializeSurveyAnswersSummaryStore(IUnityContainer container)
        {
            RegisterSurveyAnswersSummaryStore(container);

            container.Resolve<ISurveyAnswersSummaryStore>().Initialize();
        }

        public static void InitializeSurveyStore(IUnityContainer container)
        {
            RegisterSurveyStore(container);

            container.Resolve<ISurveyStore>().Initialize();
        }

        public static void InitializeTenantFilterStore(IUnityContainer container)
        {
            RegisterTenantFilterStore(container);

            container.Resolve<ITenantFilterStore>().Initialize();
        }

        public static void InitializeTenantStore(IUnityContainer container)
        {
            RegisterTenantStore(container);

            container.Resolve<ITenantStore>().Initialize();
        }

        public static void InitializeUserDeviceStore(IUnityContainer container)
        {
            RegisterUserDeviceStore(container);

            container.Resolve<IUserDeviceStore>().Initialize();
        }

        public static void RegisterFilteringService(IUnityContainer container)
        {
            container.RegisterType<IFilteringService, FilteringService>(
                new InjectionConstructor(
                    new ResolvedArrayParameter<ISurveyFilter>(new ResolvedParameter<ISurveyFilter>("tenantFilter"))));

            container.RegisterType<ISurveyFilter, TenantFilter>("tenantFilter");

            RegisterSurveyStore(container);
            RegisterTenantFilterStore(container);
            RegisterTenantStore(container);
        }

        public static void RegisterMediaAnswerStore(IUnityContainer container)
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            container.RegisterInstance(account);

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
        }

        public static void RegisterNewSurveyNotificationCommand(IUnityContainer container)
        {
            RegisterSurveyStore(container);
            RegisterFilteringService(container);
            RegisterUserDeviceStore(container);
            RegisterPushNotification(container);
        }

        public static void RegisterPushNotification(IUnityContainer container)
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            container.RegisterInstance(account);

            container.RegisterType<IPushNotification, PushNotification>();
        }

        public static void RegisterSurveyAnswerStore(IUnityContainer container)
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            container.RegisterInstance(account);

            container.RegisterType<ISurveyAnswerStore, SurveyAnswerStore>();

            // Container for resolving the survey answer containers
            var surveyAnswerBlobContainerResolver = new UnityContainer();

            surveyAnswerBlobContainerResolver.RegisterInstance(account);

            surveyAnswerBlobContainerResolver.RegisterType<IAzureBlobContainer<SurveyAnswer>, EntitiesBlobContainer<SurveyAnswer>>(
                new InjectionConstructor(typeof(CloudStorageAccount), typeof(string)));

            container.RegisterType<ISurveyAnswerContainerFactory, SurveyAnswerContainerFactory>(
                new InjectionConstructor(surveyAnswerBlobContainerResolver));

            container.RegisterType<IAzureQueue<SurveyAnswerStoredMessage>, AzureQueue<SurveyAnswerStoredMessage>>(
                new InjectionConstructor(typeof(CloudStorageAccount), AzureConstants.Queues.SurveyAnswerStored));

            container.RegisterType<IAzureBlobContainer<List<string>>, EntitiesBlobContainer<List<string>>>(
                new InjectionConstructor(typeof(CloudStorageAccount), AzureConstants.BlobContainers.SurveyAnswersLists));

            RegisterMediaAnswerStore(container);
        }

        public static void RegisterSurveyAnswerTransferStore(IUnityContainer container)
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            container.RegisterInstance(account);

            container.RegisterType<ISurveyTransferStore, SurveyTransferStore>();

            container.RegisterType<IAzureQueue<SurveyTransferMessage>, AzureQueue<SurveyTransferMessage>>(
                new InjectionConstructor(typeof(CloudStorageAccount), AzureConstants.Queues.SurveyTransferRequest));
        }

        public static void RegisterSurveyAnswersSummaryStore(IUnityContainer container)
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            container.RegisterInstance(account);

            container.RegisterType<ISurveyAnswersSummaryStore, SurveyAnswersSummaryStore>();

            container.RegisterType<IAzureBlobContainer<SurveyAnswersSummary>, EntitiesBlobContainer<SurveyAnswersSummary>>(
                new InjectionConstructor(typeof(CloudStorageAccount), AzureConstants.BlobContainers.SurveyAnswersSummaries));
        }

        public static void RegisterSurveySqlStore(IUnityContainer container)
        {
            container.RegisterType<ISurveySqlStore, SurveySqlStore>();
        }

        public static void RegisterSurveyStore(IUnityContainer container)
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            container.RegisterInstance(account);

            container.RegisterType<ISurveyStore, SurveyStore>();

            container.RegisterType<IAzureTable<SurveyRow>, AzureTable<SurveyRow>>(
                new InjectionConstructor(typeof(CloudStorageAccount), AzureConstants.Tables.Surveys));

            container.RegisterType<IAzureTable<QuestionRow>, AzureTable<QuestionRow>>(
                new InjectionConstructor(typeof(CloudStorageAccount), AzureConstants.Tables.Questions));

            container.RegisterType<IAzureQueue<NewSurveyMessage>, AzureQueue<NewSurveyMessage>>(
                new InjectionConstructor(typeof(CloudStorageAccount), AzureConstants.Queues.NewSurveyStored));
        }

        public static void RegisterTenantFilterStore(IUnityContainer container)
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            container.RegisterInstance(account);

            container.RegisterType<ITenantFilterStore, TenantFilterStore>();

            container.RegisterType<IAzureTable<TenantFilterRow>, AzureTable<TenantFilterRow>>(
                new InjectionConstructor(typeof(CloudStorageAccount), AzureConstants.Tables.TenantFilter));
        }

        public static void RegisterTenantStore(IUnityContainer container)
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            container.RegisterInstance(account);

            container.RegisterType<ITenantStore, TenantStore>(
                new InjectionConstructor(
                    new ResolvedParameter<IAzureBlobContainer<Tenant>>(),
                    new ResolvedParameter<IAzureBlobContainer<byte[]>>(AzureConstants.BlobContainers.Logos)));

            container.RegisterType<IAzureBlobContainer<Tenant>, EntitiesBlobContainer<Tenant>>(
                new InjectionConstructor(typeof(CloudStorageAccount), AzureConstants.BlobContainers.Tenants));

            container.RegisterType<IAzureBlobContainer<byte[]>, FilesBlobContainer>(
                AzureConstants.BlobContainers.Logos,
                new InjectionConstructor(typeof(CloudStorageAccount), AzureConstants.BlobContainers.Logos, "image/jpeg"));
        }

        public static void RegisterTransferSurveysToSqlAzureCommand(IUnityContainer container)
        {
            RegisterSurveyAnswerStore(container);
            RegisterSurveyStore(container);
            RegisterTenantStore(container);
            RegisterSurveySqlStore(container);
        }

        public static void RegisterUpdatingSurveyResultsSummaryCommand(IUnityContainer container)
        {
            container.RegisterType<IDictionary<string, SurveyAnswersSummary>, Dictionary<string, SurveyAnswersSummary>>(
                new InjectionConstructor());

            RegisterSurveyAnswerStore(container);
            RegisterSurveyAnswersSummaryStore(container);
        }

        public static void RegisterUserDeviceStore(IUnityContainer container)
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            container.RegisterInstance(account);

            container.RegisterType<IUserDeviceStore, UserDeviceStore>();

            container.RegisterType<IAzureTable<UserDeviceRow>, AzureTable<UserDeviceRow>>(
                new InjectionConstructor(typeof(CloudStorageAccount), AzureConstants.Tables.UserDevice));
        }
    }
}