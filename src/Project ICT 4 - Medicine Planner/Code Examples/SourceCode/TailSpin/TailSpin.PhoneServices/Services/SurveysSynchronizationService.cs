//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using System.Linq;
using System.Net;
using Microsoft.Phone.Reactive;
using Microsoft.Phone.Shell;
using TailSpin.PhoneServices.Services.Stores;
using TailSpin.PhoneServices.Services.SurveysService;

namespace TailSpin.PhoneServices.Services
{
    public class SurveysSynchronizationService : ISurveysSynchronizationService
    {
        public const string GetSurveysTask = "GetNewSurveys";
        public const string SaveSurveyAnswersTask = "SaveSurveyAnswers";
        private readonly Func<ISurveysServiceClient> surveysServiceClientFactory;
        private readonly ISurveyStoreLocator surveyStoreLocator;

        public SurveysSynchronizationService(
                Func<ISurveysServiceClient> surveysServiceClientFactory,
                ISurveyStoreLocator surveyStoreLocator)
        {
            this.surveysServiceClientFactory = surveysServiceClientFactory;
            this.surveyStoreLocator = surveyStoreLocator;
        }

        public IObservable<TaskCompletedSummary> GetNewSurveys()
        {
            return GetNewSurveys(surveyStoreLocator.GetStore());
        }

        public IObservable<TaskCompletedSummary> GetNewSurveys(ISurveyStore surveyStore)
        {
            var getNewSurveys =
                surveysServiceClientFactory()
                    .GetNewSurveys(surveyStore.LastSyncDate)
                    .Select(
                        surveys =>
                        {
                            surveyStore.SaveSurveyTemplates(surveys);
                            if (surveys.Count() > 0)
                            {
                                surveyStore.LastSyncDate = surveys.Max(s => s.CreatedOn).ToString("s");
                            }

                            var tile = ShellTile.ActiveTiles.First();
                            var tileData = new StandardTileData()
                            {
                                Count = this.UnopenedSurveyCount
                            };
                            tile.Update(tileData);                         

                            return new TaskCompletedSummary
                            {
                                Task = GetSurveysTask,
                                Result = TaskSummaryResult.Success,
                                Context = surveys.Count().ToString()
                            };
                        })
                    .Catch(
                        (Exception exception) =>
                        {
                            if (exception is WebException)
                            {
                                var webException = exception as WebException;
                                var summary = ExceptionHandling.GetSummaryFromWebException(GetSurveysTask, webException);
                                return Observable.Return(summary);
                            }

                            if (exception is UnauthorizedAccessException)
                            {
                                return Observable.Return(new TaskCompletedSummary { Task = GetSurveysTask, Result = TaskSummaryResult.AccessDenied });
                            }

                            throw exception;
                        });

            return getNewSurveys;
        }

        public int UnopenedSurveyCount
        {
            get
            {
                if(surveyStoreLocator.GetStore().AllSurveys != null)
                    return surveyStoreLocator.GetStore().AllSurveys.UnopenedCount;

                return 0;
            }
            set
            {
                if (surveyStoreLocator.GetStore().AllSurveys != null) 
                {
                    surveyStoreLocator.GetStore().AllSurveys.UnopenedCount = value;
                    surveyStoreLocator.GetStore().SaveUnopenedCount();
                }
            }
        }

        public IObservable<TaskCompletedSummary> UploadSurveys()
        {
            return UploadSurveys(surveyStoreLocator.GetStore());
        }

        public IObservable<TaskCompletedSummary> UploadSurveys(ISurveyStore surveyStore)
        {
            var surveyAnswers = surveyStore.GetCompleteSurveyAnswers();
            var saveSurveyAnswers =
                Observable.Return(
                    new TaskCompletedSummary
                    {
                        Task = SaveSurveyAnswersTask,
                        Result = TaskSummaryResult.Success,
                        Context = 0.ToString()
                    });

            if (surveyAnswers.Count() > 0)
            {
                saveSurveyAnswers =
                this.surveysServiceClientFactory()
                    .SaveSurveyAnswers(surveyAnswers)
                    .Select(
                        unit =>
                        {
                            var sentAnswersCount = surveyAnswers.Count();
                            surveyStore.DeleteSurveyAnswers(surveyAnswers);
                            return new TaskCompletedSummary
                            {
                                Task = SaveSurveyAnswersTask,
                                Result = TaskSummaryResult.Success,
                                Context = sentAnswersCount.ToString()
                            };
                        })
                    .Catch(
                        (Exception exception) =>
                        {
                            if (exception is WebException)
                            {
                                var webException = exception as WebException;
                                var summary = ExceptionHandling.GetSummaryFromWebException(SaveSurveyAnswersTask, webException);
                                return Observable.Return(summary);
                            }

                            if (exception is UnauthorizedAccessException)
                            {
                                return Observable.Return(new TaskCompletedSummary { Task = SaveSurveyAnswersTask, Result = TaskSummaryResult.AccessDenied });
                            }

                            throw exception;
                        });
            }

            return saveSurveyAnswers;
        }

        public IObservable<TaskCompletedSummary[]> StartSynchronization()
        {
            var surveyStore = surveyStoreLocator.GetStore();

            var getNewSurveys = GetNewSurveys(surveyStore);
            var saveSurveyAnswers = UploadSurveys(surveyStore);

            return Observable.ForkJoin(getNewSurveys, saveSurveyAnswers);
        }
    }
}
