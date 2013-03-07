//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Services.Surveys.Surveys
{
    public class QuestionAnswerDto
    {
        public string QuestionText { get; set; }

        public string PossibleAnswers { get; set; }

        public string QuestionType { get; set; }

        public string Answer { get; set; }
    }
}