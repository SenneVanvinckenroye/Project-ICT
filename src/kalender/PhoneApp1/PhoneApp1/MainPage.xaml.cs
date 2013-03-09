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
using Newtonsoft.Json.Linq;

namespace PhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        private static MobileServiceTable<TodoItem> todoItemTable = App.MobileServiceClient.GetTable<TodoItem>();

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Service.Read();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Service.Insert(txtbox.Text);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Service.Read();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            lstbox.Items.Clear();
            foreach (TodoItem todoItem in Service.Load())
            {
                lstbox.Items.Add(todoItem);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Service.Delete((TodoItem)lstbox.SelectedItem);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Service.Query(txtbox.Text);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page1.xaml", UriKind.Relative));
        }




    }
    public class TodoItem
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public override string ToString()
        {
            return Text;
        }
    }
}