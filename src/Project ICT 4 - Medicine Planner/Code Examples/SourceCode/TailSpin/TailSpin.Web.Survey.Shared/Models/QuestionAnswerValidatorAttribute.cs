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
    public sealed class QuestionAnswerValidatorAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            var questionAnswer = value as QuestionAnswer;

            if (questionAnswer == null)
            {
                return base.IsValid(value);
            }

            if (questionAnswer.QuestionType == QuestionType.Picture ||
                questionAnswer.QuestionType == QuestionType.Voice)
            {
                return true;
            }

            return base.IsValid(value);
        }
    }
}