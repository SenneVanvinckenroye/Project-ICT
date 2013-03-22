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
using System.ServiceModel;


namespace MedAgent_0_1
{
    public partial class MainPage : PhoneApplicationPage
    {
        MedPlanServiceReference.MedPlanServiceClient client;
        public MainPage()
        {
            InitializeComponent();
            //initialize wcf service client

            client = new MedPlanServiceReference.MedPlanServiceClient();
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
            if (EmailBox.Text != "" && Paswoord_pswdbx.Password != "")
            {
                //using wcf service to login
                try
                {
                    client.LoginAsync(EmailBox.Text, Paswoord_pswdbx.Password);
                    client.LoginCompleted += new EventHandler<MedPlanServiceReference.LoginCompletedEventArgs>(client_LoginCompleted);
                }
                catch(EndpointNotFoundException)
                {
                    error_txblck.Text = "[Error]\nService problems";
                    ErrorPopup.IsOpen = true;
                }
            }
        }

        void client_LoginCompleted(object sender, MedPlanServiceReference.LoginCompletedEventArgs e)
        {
            if (e.Result != "")//voornaam returned door login functie
            {
                NavigationService.Navigate(new Uri(string.Format("/AddMedicationPage.xaml"), UriKind.Relative));
                LogInPopup.IsOpen = false;
            }
            else
            {
                error_txblck.Text = "Email and or password \n not found";
                ErrorPopup.IsOpen = true;
            }
        }

        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	LogInPopup.IsOpen = false;
        }

        private void confirmButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ErrorPopup.IsOpen = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri(string.Format("/DoctorView1.xaml"), UriKind.Relative));
        }

    }
	
	
}
