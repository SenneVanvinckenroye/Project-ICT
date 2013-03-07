//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using Microsoft.Practices.Prism.ViewModel;

namespace TailSpin.PhoneServices.Models
{
    public class QuestionOption : NotificationObject
    {
        private bool isSelected;
        private string text;

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }

            set
            {
                isSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }

        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
                RaisePropertyChanged(() => Text);
            }
        }
    }
}
