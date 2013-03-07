//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System.Runtime.Serialization;

namespace TailSpin.Services.Surveys.Surveys
{
    [DataContract]
    public class QuestionDto
    {
        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string PossibleAnswers { get; set; }
    }
}