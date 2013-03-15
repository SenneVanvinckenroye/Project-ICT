using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;


namespace MedAgent_0_1
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void PatientButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //NavigationService.Navigate(new Uri(string.Format("/PatientLogInPage.xaml"), UriKind.Relative));

            /*Popup Mypopup = new Popup();
            Mypopup.Height = 300;
            Mypopup.Width = 400;
            Mypopup.VerticalOffset = 100;
            PopUpUserControl Mycontrol = new PopUpUserControl();
            Mypopup.Child = Mycontrol;
            Mypopup.IsOpen = true;*/

            LogInPopup.IsOpen = true;
            


        }

        private void LogInButtonPatient_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	NavigationService.Navigate(new Uri(string.Format("/AddMedicationPage.xaml"), UriKind.Relative));
            LogInPopup.IsOpen = false;
        }

        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	LogInPopup.IsOpen = false;
        }

    }
	
	
}
