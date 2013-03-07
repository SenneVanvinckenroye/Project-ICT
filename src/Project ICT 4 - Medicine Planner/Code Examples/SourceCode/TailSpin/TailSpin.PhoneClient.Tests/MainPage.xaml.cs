//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using Microsoft.Phone.Controls;
using Microsoft.Silverlight.Testing;
using Microsoft.Silverlight.Testing.Harness;
using TailSpin.Phone.TestSupport;

namespace TailSpin.PhoneClient.Tests
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            this.InitializeComponent();
            LogProvider fileLogProvider = new FileLogProvider();
            var settings = UnitTestSystem.CreateDefaultSettings();
            settings.LogProviders.Add(fileLogProvider);

            Content = UnitTestSystem.CreateTestPage(settings);
            BackKeyPress += (x, xe) => xe.Cancel = (Content as IMobileTestPage).NavigateBack();
        }
    }
}