//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using System.Windows;
using Microsoft.Phone.Reactive;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using TailSpin.Phone.Adapters;
using TailSpin.PhoneServices.Services;
using TailSpin.PhoneServices.Services.Clients;
using TailSpin.PhoneServices.Services.Stores;
using TailSpin.PhoneServices.Services.SurveysService;

namespace TailSpin.PhoneAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        private static volatile bool classInitialized;
        private ISettingsStore settingsStore;
        
        public ScheduledAgent()
        {
            this.settingsStore = new SettingsStore(new ProtectDataAdapter());

            if (!classInitialized)
            {
                classInitialized = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {
                    Application.Current.UnhandledException += ScheduledAgent_UnhandledException;
                });
            }
        }

        private void ScheduledAgent_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        protected override void OnInvoke(ScheduledTask task)
        {
            if (task is PeriodicTask)
            {
                RunPeriodicTask(task);
            }
            else if(task is ResourceIntensiveTask)
            {
                RunResourceIntensiveTask(task);
            }
        }

        private void SyncCompleted(TaskCompletedSummary taskSummary)
        {
            int newCount;

            if (taskSummary != null && int.TryParse(taskSummary.Context, out newCount) && newCount > 0)
            {
                var toast = new ShellToast();
                toast.Title = TaskCompletedSummaryStrings.GetDescriptionForSummary(taskSummary);
                toast.Content = "";
                toast.Show();
            }

            NotifyComplete();
        }

        private void SyncFailed(Exception ex)
        {
            Abort();
        }

        private void RunPeriodicTask(ScheduledTask task)
        {
#if ONLY_PHONE
            var surveyServiceClient = new SurveysServiceClientMock(settingsStore);
#else
            var httpClient = new HttpClient();
            var surveyServiceClient = new SurveysServiceClient(new Uri("http://127.0.0.1:8080/Survey/"), settingsStore, httpClient);
#endif
            var surveyStoreLocator = new SurveyStoreLocator(settingsStore, storeName => new SurveyStore(storeName));
            var synchronizationService = new SurveysSynchronizationService(() => surveyServiceClient, surveyStoreLocator);

            synchronizationService
                .GetNewSurveys()
                .ObserveOnDispatcher()
                .Subscribe(SyncCompleted, SyncFailed);

#if DEBUG
            ScheduledActionService.LaunchForTest(task.Name, TimeSpan.FromMinutes(3));
#endif
        }

        /// <summary>
        /// This method executes SynchronizationService.UploadSurveys through a ResourceIntensiveTask.
        /// It may be more appropriate in your situation to use the background file transfter service
        /// if the data to be transfered can be grouped into separate files that can be queued up.
        /// </summary>
        /// <param name="task"></param>
        private void RunResourceIntensiveTask(ScheduledTask task)
        {
#if ONLY_PHONE
            var surveyServiceClient = new SurveysServiceClientMock(settingsStore);
#else
            var surveyServiceClient = new SurveysServiceClient(new Uri("http://127.0.0.1:8080/Survey/"), settingsStore, new HttpClient());
#endif
            var surveyStoreLocator = new SurveyStoreLocator(settingsStore, storeName => new SurveyStore(storeName));
            var synchronizationService = new SurveysSynchronizationService(() => surveyServiceClient, surveyStoreLocator);

            synchronizationService
                .UploadSurveys()
                .ObserveOnDispatcher()
                .Subscribe(SyncCompleted, SyncFailed);

#if DEBUG
            ScheduledActionService.LaunchForTest(task.Name, TimeSpan.FromMinutes(3));
#endif
        }
    }
}
