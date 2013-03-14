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

namespace MediAgent
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            //this.Loaded += MainPage_Loaded;
        }

        //void MainPage_Loaded(object sender, RoutedEventArgs e)
        //{
        //    Authenticate();
        //    //RefreshTodoItems();
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/DoctorView1.xaml", UriKind.Relative));
        }

        //private MobileServiceClient user;
        //private void Authenticate()
        //{
        //    while (user == null)
        //    {
        //        string message;
        //        try
        //        {
        //            user = App.MobileServiceClient
        //                .Login(MobileServiceAuthenticationProvider);
        //            message =
        //                string.Format("You are now logged in - {0}", user);
        //        }
        //        catch (InvalidOperationException)
        //        {
        //            message = "You must log in. Login Required";
        //        }


        //        MessageBox.Show(message);
        //    }


        //}
    }
}