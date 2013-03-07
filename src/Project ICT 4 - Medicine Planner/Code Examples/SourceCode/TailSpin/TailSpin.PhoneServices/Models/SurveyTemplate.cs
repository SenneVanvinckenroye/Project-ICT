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

using TailSpin.PhoneServices.Services;

namespace TailSpin.PhoneServices.Models
{
    public class SurveyTemplate
    {
        public SurveyTemplate()
        {
            Questions = new List<Question>();
        }

        public string IconUrl { get; set; }

        public string Tenant { get; set; }

        public string Title { get; set; }

        public string SlugName { get; set; }

        public DateTime CreatedOn { get; set; }

        public List<Question> Questions { get; set; }

        public bool IsNew { get; set; }

        public bool IsFavorite { get; set; }

        public bool IsLocal { get; set; }

        public int Completeness { get; set; }

        public int Length { get; set; }

        public SurveyAnswer CreateSurveyAnswers()
        {
            var survey = new SurveyAnswer
            {
                Answers = new List<QuestionAnswer>(),
                SlugName = SlugName,
                Tenant = Tenant,
                Title = Title
            };
            foreach (var question in Questions)
            {
                survey.Answers.Add(new QuestionAnswer
                {
                    QuestionType = question.Type,
                    QuestionText = question.Text,
                    PossibleAnswers = new List<string>(question.PossibleAnswers)
                });
            }

            return survey;
        }
    }
}

