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
using System.Runtime.Serialization;

namespace TailSpin.Services.Surveys.Surveys
{
    [DataContract]
    public class SurveyDto
    {
        public SurveyDto()
        {
            this.Questions = new List<QuestionDto>();
        }

        [DataMember]
        public string SlugName { get; set; }

        [DataMember]
        public string Tenant { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string IconUrl { get; set; }

        [DataMember]
        public int Length { get; set; }

        [DataMember]
        public List<QuestionDto> Questions { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }
    }
}