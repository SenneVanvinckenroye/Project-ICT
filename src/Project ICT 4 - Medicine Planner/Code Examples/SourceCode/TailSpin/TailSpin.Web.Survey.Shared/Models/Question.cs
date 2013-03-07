//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Web.Survey.Shared.Models
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public enum QuestionType
    {
        SimpleText,
        MultipleChoice,
        FiveStars,
        Picture,
        Voice
    }

    [Serializable]
    [RequiredAnswer(ErrorMessage = "* Answers must be supplied for the question.")]
    public class Question
    {
        public Question()
        {
            this.Type = QuestionType.SimpleText;
        }

        [Required(ErrorMessage = "* You must provide a Question.")]
        [DisplayName("Question")]
        public string Text { get; set; }

        public QuestionType Type { get; set; }

        [DisplayName("Answers")]
        public string PossibleAnswers { get; set; }
    }
}