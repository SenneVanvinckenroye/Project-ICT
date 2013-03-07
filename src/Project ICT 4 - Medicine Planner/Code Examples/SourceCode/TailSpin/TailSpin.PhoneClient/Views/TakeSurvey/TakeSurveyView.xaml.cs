//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using TailSpin.PhoneClient.ViewModels;

namespace TailSpin.PhoneClient.Views.TakeSurvey
{
    public partial class TakeSurveyView
    {
        private bool loaded;

        // Constructor
        public TakeSurveyView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (((TakeSurveyViewModel)this.DataContext).TemplateViewModel == null)
            {
                string surveyId;
                if (NavigationContext.QueryString.ContainsKey("surveyID"))
                {
                    surveyId = NavigationContext.QueryString["surveyID"];
                }
                else
                {
                    surveyId = (string)PhoneApplicationService.Current.State["TakeSurveyId"];
                }

                ((TakeSurveyViewModel)this.DataContext).Initialize(surveyId);
            }
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            ((TakeSurveyViewModel)this.DataContext).SaveDirtyDataAndCleanUp();
            base.OnBackKeyPress(e);
        }

        private static string ProcessTitle(string title)
        {
            // Trim the title if it's too long and add ellipsis in that case.
            if (title.Length > 40)
            {
                title = title.Substring(0, 40) + "...";
            }
            return title.ToUpper();
        }

        private void PivotSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (this.loaded)
            {
                ((TakeSurveyViewModel)this.DataContext).SelectedPivotIndex = this.questionsPivot.SelectedIndex;
            }
        }

        private void ControlLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var vm = (TakeSurveyViewModel)this.DataContext;
            this.questionsPivot.SelectedItem = 
                this.questionsPivot.Items[vm.SelectedPivotIndex];
            this.loaded = true;
            this.questionsPivot.Title =
                ProcessTitle(string.Format("{0} - {1}", vm.TemplateViewModel.Title, vm.TemplateViewModel.Tenant));
        }
    }
}
