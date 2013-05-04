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
            if (App.InternetOn())
            {
                client = new MedCareCloudServiceReference.MedPlanServiceClient();
            }
            else
            {
                App.ConnectErrorMsg();
            }
        }

        private void LogInButtonPatient_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (EmailBox.Text != "" && Paswoord_pswdbx.Password != "")
            {
                if (App.InternetOn())
                {
                    //using wcf service to login
                    try
                    {
                        client.LoginAsync(EmailBox.Text, MD5Core.GetHashString(Paswoord_pswdbx.Password));
                        client.LoginCompleted += new EventHandler<MedCareCloudServiceReference.LoginCompletedEventArgs>(client_LoginCompleted);
                    }
                    catch (EndpointNotFoundException)
                    {
                        error_txblck.Text = "[Error]\nService problems";
                        ErrorPopup.IsOpen = true;
                    }
                }
                else
                {
                    App.ConnectErrorMsg();
                }
            }
            else
            {
                MessageBox.Show("Missing entries!");
            }
        }
        
        public static string userFName, userLName, userEmail;//global vars to reach userdata in other pages
        public static int userMemberID;

        void client_LoginCompleted(object sender, MedCareCloudServiceReference.LoginCompletedEventArgs e)
        {
            if(e.Result == null)
            {
                error_txblck.Text = "Email and or password \n not found";
                ErrorPopup.IsOpen = true;
            }
            else
            {
                userFName = e.Result.FName;
                userLName = e.Result.LName;
                userEmail = e.Result.email;
                userMemberID = e.Result.MemberID;
                switch(e.Result.UserType)
                {
                    case 'p'://patient
                    {
                        NavigationService.Navigate(new Uri("/PatientFile.xaml?isPatient=true", UriKind.Relative));
                        break;
                    }
                    case 'd'://dokter
                    {
                        client.GetDocInfoAsync(userMemberID);
                        client.GetDocInfoCompleted += new EventHandler<MedCareCloudServiceReference.GetDocInfoCompletedEventArgs>(client_GetDocInfoCompleted);
                        
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

        void client_GetDocInfoCompleted(object sender, MedCareCloudServiceReference.GetDocInfoCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                App.PublicDoctor.DocID = e.Result.DoctorID;
                App.PublicDoctor.MemberID = e.Result.MemberID;
                App.PublicDoctor.Speciality = e.Result.Speciality;

                NavigationService.Navigate(new Uri(string.Format("/DoctorView1.xaml"), UriKind.Relative));
            }
            else
            {
                MessageBox.Show("Server error:\n\rWe failed to retrieve your data\n\r\n\rPlease retry or contact an Admin.");
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
