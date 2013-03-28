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
            switch(e.Result)
            {
                case "":
                    {
                        error_txblck.Text = "Email and or password \n not found";
                        ErrorPopup.IsOpen = true;
                        break;
                    }
                case "patient":
                    {
                        NavigationService.Navigate(new Uri(string.Format("/PatientFile.xaml"), UriKind.Relative));
                        break;
                    }
                case "doctor":
                    {
                        NavigationService.Navigate(new Uri(string.Format("/DoctorView1.xaml"), UriKind.Relative));
                        break;
                    }
                case "nurse":
                    {
                        NavigationService.Navigate(new Uri(string.Format("/DoctorView1.xaml"), UriKind.Relative));
                        break;
                    }
            }
        }

        private void confirmButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ErrorPopup.IsOpen = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri(string.Format("/DoctorView1.xaml"), UriKind.Relative));
        }

        private void byPass_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri(string.Format("/DoctorView1.xaml"), UriKind.Relative));
        }

    }
	
	
}
