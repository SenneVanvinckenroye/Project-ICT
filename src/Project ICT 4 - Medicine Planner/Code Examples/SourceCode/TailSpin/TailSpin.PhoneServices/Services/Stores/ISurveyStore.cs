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
    public interface ISurveyStore
    {
        string LastSyncDate { get; set; }
        SurveysList AllSurveys { get; set; }
        IEnumerable<SurveyTemplate> GetSurveyTemplates();
        IEnumerable<SurveyAnswer> GetCompleteSurveyAnswers();
        void SaveSurveyTemplates(IEnumerable<SurveyTemplate> surveys);
        void SaveSurveyAnswer(SurveyAnswer answer);
        SurveyAnswer GetCurrentAnswerForTemplate(SurveyTemplate template);
        void DeleteSurveyAnswers(IEnumerable<SurveyAnswer> surveyAnswers);
        void SaveStore();
        void SaveUnopenedCount();
        void MarkSurveyTemplateRead(SurveyTemplate template);

        event EventHandler SurveyAnswerSaved;
    }
}
