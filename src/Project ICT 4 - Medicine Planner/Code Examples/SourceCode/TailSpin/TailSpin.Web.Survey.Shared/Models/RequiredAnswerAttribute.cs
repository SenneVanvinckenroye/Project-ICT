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
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Class)]
    public class RequiredAnswerAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var question = value as Question;

            if (question == null)
            {
                return base.IsValid(value);
            }

            if (question.Type == QuestionType.MultipleChoice)
            {
                return !string.IsNullOrEmpty(question.PossibleAnswers);
            }

            return true;
        }
    }
}