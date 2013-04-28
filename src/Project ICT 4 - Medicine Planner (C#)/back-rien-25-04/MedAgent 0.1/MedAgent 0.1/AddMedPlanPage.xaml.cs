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
    public partial class AddMedPlanPage : PhoneApplicationPage
    {
        public AddMedPlanPage()
        {
            InitializeComponent();
        }

        private void course_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            switch ((int)(course.Value))
            {
                case 0: repeatInfo_txt.Text = "Daily";
                    break;
                case 1: repeatInfo_txt.Text = "Every 2 days";
                    break;
                case 2: repeatInfo_txt.Text = "Every 3 days";
                    break;
                case 3: repeatInfo_txt.Text = "Every 4 days";
                    break;
                case 4: repeatInfo_txt.Text = "Every 5 days";
                    break;
                case 5: repeatInfo_txt.Text = "Weekly";
                    break;
                default: repeatInfo_txt.Text = "error";
                    break;
            }
        }
    }
}
