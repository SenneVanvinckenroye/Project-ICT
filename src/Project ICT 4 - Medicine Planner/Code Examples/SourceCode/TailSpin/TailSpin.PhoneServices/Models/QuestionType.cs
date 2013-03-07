//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;

namespace TailSpin.PhoneServices.Models
{
    public enum QuestionType
    {
        SimpleText,
        MultipleChoice,
        FiveStars,
        Picture,
        Voice
    }

    public static class QuestionTypeParser
    {
        public static QuestionType Parse(string p)
        {
            var parsedValue = QuestionType.SimpleText;

            try
            {
                parsedValue = (QuestionType)Enum.Parse(typeof(QuestionType), p, true);
            }
            catch { }
            return parsedValue;
        }
    }
}
