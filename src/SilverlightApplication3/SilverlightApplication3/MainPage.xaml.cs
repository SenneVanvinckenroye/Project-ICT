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

namespace SilverlightApplication3
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();

            ServiceReference1.MedPlanServiceClient client = new ServiceReference1.MedPlanServiceClient();
            client.GetAllUsersCompleted += client_GetAllUsersCompleted;
            client.GetAllUsersAsync();
        }

        void client_GetAllUsersCompleted(object sender, ServiceReference1.GetAllUsersCompletedEventArgs e)
        {
            dg.ItemsSource = e.Result;
        }
    }
}
