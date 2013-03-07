//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System.Windows;

namespace TailSpin.PhoneClient.Adapters
{
    public class MessageBoxAdapter : IMessageBox
    {
        public void Show(string message)
        {
            MessageBox.Show(message);
        }

        public void Show(string messageBoxText, string caption, MessageBoxButton button)
        {
            MessageBox.Show(messageBoxText, caption, button);
        }
    }
}
