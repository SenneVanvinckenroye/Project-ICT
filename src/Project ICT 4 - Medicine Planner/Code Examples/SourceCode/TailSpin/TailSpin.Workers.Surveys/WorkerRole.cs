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
    using System.Linq;
    using System.Net;
    using System.Threading;
    using Commands;
    using Microsoft.Practices.Unity;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using Web.Survey.Shared.QueueMessages;
    using Web.Survey.Shared.Stores;
    using Web.Survey.Shared.Stores.AzureStorage;

    public class WorkerRole : RoleEntryPoint
    {
        private IUnityContainer container;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "This is the default code from the project template for the Windows Azure SDK.")]
        public override bool OnStart()
        {
            // The number of connections depends on the particular usage in each application
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
                                                                 configSetter(RoleEnvironment.GetConfigurationSettingValue(configName)));
            
            this.container = new UnityContainer();
            ContainerBootstraper.RegisterTypes(this.container);

            this.container.Resolve<ISurveyAnswerStore>().Initialize();
            this.container.Resolve<ISurveyAnswersSummaryStore>().Initialize();
            this.container.Resolve<ITenantStore>().Initialize();
            this.container.Resolve<ISurveyStore>().Initialize();
            this.container.Resolve<ISurveyTransferStore>().Initialize();

            return base.OnStart();
        }

        public override void Run()
        {
            //// The time interval for checking the queues have to be tuned depending on the scenario and the expected workload
            var updatingSurveyResultsSummaryJob = this.container.Resolve<UpdatingSurveyResultsSummaryCommand>();
            var surveyAnswerStoredQueue = this.container.Resolve<IAzureQueue<SurveyAnswerStoredMessage>>();
            BatchProcessingQueueHandler.For(surveyAnswerStoredQueue).Every(TimeSpan.FromSeconds(10))
                                       .Do(updatingSurveyResultsSummaryJob);

            var transferQueue = this.container.Resolve<IAzureQueue<SurveyTransferMessage>>();
            var transferCommand = this.container.Resolve<TransferSurveysToSqlAzureCommand>();
            QueueHandler.For(transferQueue).Every(TimeSpan.FromSeconds(5)).Do(transferCommand);

            while (true)
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }

        private static void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                e.Cancel = true;
            }
        }
    }
}
