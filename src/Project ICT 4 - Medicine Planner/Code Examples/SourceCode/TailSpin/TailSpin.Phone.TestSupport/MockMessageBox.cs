//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System.Windows;
using TailSpin.PhoneClient.Adapters;

namespace TailSpin.Phone.TestSupport
{
    public class MockMessageBox : IMessageBox
    {
        public string MessageBoxText { get; set; }

        public void Show(string messageBoxText)
        {
            MessageBoxText = messageBoxText;
        }

        public void Show(string messageBoxText, string caption, MessageBoxButton button)
        {
            MessageBoxText = messageBoxText;
        }
    }
}
