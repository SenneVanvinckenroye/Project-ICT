//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System.Collections.Generic;
using Microsoft.Practices.Prism.ViewModel;

namespace TailSpin.PhoneServices.Models
{
    public class QuestionAnswer : NotificationObject
    {
        private string backingValue;

        public QuestionType QuestionType { get; set; }

        public string QuestionText { get; set; }

        public string Value
        {
            get
            {
                return backingValue;
            }

            set
            {
                backingValue = value;
                RaisePropertyChanged(() => Value);
            }
        }

        public List<string> PossibleAnswers { get; set; }
    }
}
