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
        MedCareCloudServiceReference.MedPlanServiceClient client;
        public MainPage()
        {
            InitializeComponent();
            //initialize wcf service client

            client = new MedCareCloudServiceReference.MedPlanServiceClient();
        }

        private void LogInButtonPatient_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (EmailBox.Text != "" && Paswoord_pswdbx.Password != "")
            {
                //using wcf service to login
                try
                {
                    client.LoginAsync(EmailBox.Text, MD5Core.GetHashString(Paswoord_pswdbx.Password));
                    client.LoginCompleted += new EventHandler<MedCareCloudServiceReference.LoginCompletedEventArgs>(client_LoginCompleted);
                }
                catch(EndpointNotFoundException)
                {
                    error_txblck.Text = "[Error]\nService problems";
                    ErrorPopup.IsOpen = true;
                }
            }
        }

        public static Patient PublicPatient;
        public static string userFName,userLName,userEmail;//global vars to reach userdata in other pages
        void client_LoginCompleted(object sender, MedCareCloudServiceReference.LoginCompletedEventArgs e)
        {
            if(e.Result == null)
            {
                error_txblck.Text = "Email and or password \n not found";
                ErrorPopup.IsOpen = true;
            }
            else
            {
                userFName = e.Result.FirstName;
                userLName = e.Result.Name;
                userEmail = e.Result.email;
                switch(e.Result.UserType)
                {
                    case 'p'://patient
                    {
                        NavigationService.Navigate(new Uri(string.Format("/PatientFile.xaml"), UriKind.Relative));
                        break;
                    }
                    case 'd'://dokter
                    {
                        NavigationService.Navigate(new Uri(string.Format("/DoctorView1.xaml"), UriKind.Relative));
                        break;
                    }
                    case 'n'://verpleger
                    {
                        NavigationService.Navigate(new Uri(string.Format("/DoctorView1.xaml"), UriKind.Relative));
                        break;
                    }
                    default:
                    {
                        error_txblck.Text = "~Error~ in usertype";
                        ErrorPopup.IsOpen = true;
                        break;
                    }
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
