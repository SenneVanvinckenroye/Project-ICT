//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Workers.Surveys.Commands
{
    using System.Collections.Generic;
    using System.Globalization;
    using TailSpin.Web.Survey.Shared.Models;
    using TailSpin.Web.Survey.Shared.Stores;
    using Web.Survey.Shared.QueueMessages;

    public class UpdatingSurveyResultsSummaryCommand : IBatchCommand<SurveyAnswerStoredMessage>
    {
        private readonly IDictionary<string, SurveyAnswersSummary> surveyAnswersSummaryCache;
        private readonly ISurveyAnswerStore surveyAnswerStore;
        private readonly ISurveyAnswersSummaryStore surveyAnswersSummaryStore;

        public UpdatingSurveyResultsSummaryCommand(IDictionary<string, SurveyAnswersSummary> surveyAnswersSummaryCache, ISurveyAnswerStore surveyAnswerStore, ISurveyAnswersSummaryStore surveyAnswersSummaryStore)
        {
            this.surveyAnswersSummaryCache = surveyAnswersSummaryCache;
            this.surveyAnswerStore = surveyAnswerStore;
            this.surveyAnswersSummaryStore = surveyAnswersSummaryStore;
        }

        public void PreRun()
        {
           this.surveyAnswersSummaryCache.Clear();
        }

        public void Run(SurveyAnswerStoredMessage message)
        {
            this.surveyAnswerStore.AppendSurveyAnswerIdToAnswersList(
                                    message.Tenant,
                                    message.SurveySlugName,
                                    message.SurveyAnswerBlobId);

            var surveyAnswer = this.surveyAnswerStore.GetSurveyAnswer(
                                    message.Tenant,
                                    message.SurveySlugName,
                                    message.SurveyAnswerBlobId);

            var keyInCache = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", message.Tenant, message.SurveySlugName);
            SurveyAnswersSummary surveyAnswersSummary;

            if (!this.surveyAnswersSummaryCache.ContainsKey(keyInCache))
            {
                surveyAnswersSummary = new SurveyAnswersSummary(message.Tenant, message.SurveySlugName);
                this.surveyAnswersSummaryCache[keyInCache] = surveyAnswersSummary;
            }
            else
            {
                surveyAnswersSummary = this.surveyAnswersSummaryCache[keyInCache];
            }

            surveyAnswersSummary.AddNewAnswer(surveyAnswer);
        }

        public void PostRun()
        {
            foreach (var surveyAnswersSummary in this.surveyAnswersSummaryCache.Values)
            {
                var surveyAnswersSummaryInStore = this.surveyAnswersSummaryStore.GetSurveyAnswersSummary(surveyAnswersSummary.Tenant, surveyAnswersSummary.SlugName);
                surveyAnswersSummary.MergeWith(surveyAnswersSummaryInStore);
                this.surveyAnswersSummaryStore.SaveSurveyAnswersSummary(surveyAnswersSummary);
            }
        }
    }
}
