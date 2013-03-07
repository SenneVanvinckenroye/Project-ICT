//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System.Windows.Navigation;
using TailSpin.PhoneClient.ViewModels;
using Microsoft.Phone.Controls;
using System.Windows;

namespace TailSpin.PhoneClient.Views.SurveyList
{
    public partial class SurveyListView
    {
        // Constructor
        public SurveyListView()
        {
            InitializeComponent();
            OrientationChanged += SurveyListView_OrientationChanged;
        }

        void SurveyListView_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            if (e.Orientation == PageOrientation.LandscapeLeft || e.Orientation == PageOrientation.LandscapeRight)
            {
                VisualStateManager.GoToState(this, "LandscapeState", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "PortraitState", true);
            }
        }

        private void AboutMenuItem_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("Tailspin\r\nversion 1.0.0.0\r\nhttp://wp7guide.codeplex.com\r\nhttp://wp7guide.codeplex.com/privacy/", "About Tailspin", MessageBoxButton.OK);
        }
    }
}
