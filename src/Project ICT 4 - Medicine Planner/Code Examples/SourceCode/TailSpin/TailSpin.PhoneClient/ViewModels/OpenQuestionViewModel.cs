//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using TailSpin.PhoneServices.Models;

namespace TailSpin.PhoneClient.ViewModels
{
    public class OpenQuestionViewModel : QuestionViewModel
    {
        public OpenQuestionViewModel(QuestionAnswer answer)
            : base(answer)
        {
        }

        public string AnswerText
        {
            get { return this.Answer.Value; } 
            set { this.Answer.Value = value; }
        }
    }
}
