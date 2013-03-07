//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System.Collections.Generic;

namespace TailSpin.PhoneServices.Models
{
    public class Question
    {
        public Question()
        {
            PossibleAnswers = new List<string>();
        }

        public QuestionType Type { get; set; }

        public string Text { get; set; }

        public List<string> PossibleAnswers { get; set; }
    }
}
