//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System.Collections.Generic;
using System.Device.Location;
using System.Linq;

namespace TailSpin.PhoneServices.Models
{
    public class SurveyAnswer
    {
        public List<QuestionAnswer> Answers { get; set; }

        public string Tenant { get; set; }

        public string SlugName { get; set; }

        public bool IsComplete { get; set; }

        public string Title { get; set; }

        public GeoCoordinate StartLocation { get; set; }

        public GeoCoordinate CompleteLocation { get; set; }

        public int CompletedAnswers
        {
            get { return Answers.Where(a => !string.IsNullOrEmpty(a.Value)).Count(); }
        }

        public int CompletenessPercentage
        {
            get
            {
                return (Answers.Where(a => !string.IsNullOrEmpty(a.Value)).Count() * 100) / Answers.Count;
            }
        }

        public void SetAnswersFrom(SurveyAnswer source)
        {
            StartLocation = source.StartLocation;
            for (var i = 0; i < Answers.Count; i++)
            {
                Answers[i].Value = source.Answers[i].Value;
            }
        }
    }
}
