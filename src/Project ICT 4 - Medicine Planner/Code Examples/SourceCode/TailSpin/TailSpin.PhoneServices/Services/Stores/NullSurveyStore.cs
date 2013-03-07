//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using System.Collections.Generic;
using TailSpin.PhoneServices.Models;

namespace TailSpin.PhoneServices.Services.Stores
{
    public class NullSurveyStore : ISurveyStore
    {
        public string LastSyncDate { get; set; }

        public SurveysList AllSurveys { get; set; }

        public void DeleteSurveyAnswers(IEnumerable<SurveyAnswer> surveyAnswers)
        {
        }

        public IEnumerable<SurveyAnswer> GetCompleteSurveyAnswers()
        {
            return new List<SurveyAnswer>();
        }

        public SurveyAnswer GetCurrentAnswerForTemplate(SurveyTemplate template)
        {
            return new SurveyAnswer();
        }

        public IEnumerable<SurveyTemplate> GetSurveyTemplates()
        {
            return new List<SurveyTemplate>();
        }

        public void SaveStore()
        {
        }

        public void SaveUnopenedCount()
        {
            
        }

        public void MarkSurveyTemplateRead(SurveyTemplate template)
        {
        }

        public event EventHandler SurveyAnswerSaved;

        public void SaveSurveyAnswer(SurveyAnswer answer)
        {
        }

        public void SaveSurveyTemplates(IEnumerable<SurveyTemplate> surveys)
        {
        }
    }
}
