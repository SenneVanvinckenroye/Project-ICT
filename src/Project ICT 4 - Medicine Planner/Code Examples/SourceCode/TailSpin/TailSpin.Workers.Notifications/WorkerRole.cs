//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Workers.Notifications
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using Microsoft.Practices.Unity;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Diagnostics;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using TailSpin.Web.Survey.Shared;
    using Web.Survey.Shared.Handlers;
    using Web.Survey.Shared.QueueMessages;
    using Web.Survey.Shared.Stores.AzureStorage;

    public class WorkerRole : RoleEntryPoint
    {
        private IUnityContainer container;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "This is the default code from the project template for the Windows Azure SDK.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Microsoft.DisposeObjectsBeforeLosingScope", Justification = "This container is used in the controller factory and cannot be disposed.")]
        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += (sender, e) =>
            {
                if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
                {
                    // Set e.Cancel to true to restart this role instance
                    e.Cancel = true;
                }
            };

            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
                                                                configSetter(RoleEnvironment.GetConfigurationSettingValue(configName)));

            this.container = new UnityContainer();

            ComponentRegistration.RegisterNewSurveyNotificationCommand(this.container);

            return base.OnStart();
        }

        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("TailSpin.Workers.Notifications entry point called", "Information");

            var newSurvenyCommand = this.container.Resolve<NewSurveyNotificationCommand>();
            var newSurveyQueue = this.container.Resolve<IAzureQueue<NewSurveyMessage>>();
            QueueHandler.For(newSurveyQueue).Every(TimeSpan.FromSeconds(5)).Do(newSurvenyCommand);

            while (true)
            {
                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");
            }
        }
    }
}