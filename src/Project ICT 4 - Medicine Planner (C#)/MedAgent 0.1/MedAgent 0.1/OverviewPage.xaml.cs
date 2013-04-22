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
    public partial class OverviewPage : PhoneApplicationPage
    {

       


        public OverviewPage()
        {
            InitializeComponent();

            var obj = App.Current as App;


            


            //FlickrDetailPage iFlickrDetail = new FlickrDetailPage();

            
            //iFlickrDetail.UserName = ((FlickrData)DataContext).UserName.ToLower();
            //iFlickrDetail.ImageSource = ((FlickrData)DataContext).ImageSource.ToLower();
            //iFlickrDetail.PubDate = ((FlickrData)DataContext).PubDate;
            //iFlickrDetail.Message = ((FlickrData)DataContext).Message.ToLower();


            //listBoxFlickrDetail.Items.Add(iFlickrDetail); 

            //De strings van de dates en times in type DateTime zette


            //StartDate.Text = App.MedList[App.MedID].StartDate.ToShortDateString();

            EndDate.Text = obj.EndDate.ToShortDateString();

            Time1.Text = obj.Time1.ToShortTimeString();
            Time2.Text = obj.Time2.ToShortTimeString();
            Time3.Text = obj.Time3.ToShortTimeString();
            Time4.Text = obj.Time4.ToShortTimeString();
            Time5.Text = obj.Time5.ToShortTimeString();
            Time6.Text = obj.Time6.ToShortTimeString();

            Tablets1.Text = obj.Amount1.ToString();
            Tablets2.Text = obj.Amount2.ToString();
            Tablets3.Text = obj.Amount3.ToString();
            Tablets4.Text = obj.Amount4.ToString();
            Tablets5.Text = obj.Amount5.ToString();
            Tablets6.Text = obj.Amount6.ToString();
        }




        private void Button_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(new Uri(string.Format("/MedListOverview.xaml"), UriKind.Relative));
        }
    }
}
