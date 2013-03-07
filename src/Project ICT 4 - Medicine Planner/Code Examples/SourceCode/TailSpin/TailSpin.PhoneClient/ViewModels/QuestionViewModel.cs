//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using Microsoft.Practices.Prism.ViewModel;
using TailSpin.PhoneServices.Models;

namespace TailSpin.PhoneClient.ViewModels
{
    public class QuestionViewModel : NotificationObject
    {
        private readonly QuestionAnswer answer;

        public QuestionViewModel(QuestionAnswer answer)
        {
            this.answer = answer;
            this.QuestionText = answer.QuestionText;
        }

        public string Title { get; set; }

        public string QuestionText { get; private set; }

        protected QuestionAnswer Answer
        {
            get { return this.answer; }
        }
    }
}
