using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace MedAgent_0_1
{
    public partial class LogInPage : PhoneApplicationPage
    {
        public LogInPage()
        {
            InitializeComponent();
        }

        private void LogInButtonPatient_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
        }

		private void nextButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri(string.Format("/AddMedicationPage.xaml"), UriKind.Relative));
			
        }
    }
}
