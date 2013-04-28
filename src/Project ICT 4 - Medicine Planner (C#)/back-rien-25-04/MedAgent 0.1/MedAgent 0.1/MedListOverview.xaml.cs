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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace MedAgent_0_1
{
    public partial class MedListOverview : PhoneApplicationPage
    {

        //Helper Class for showing some of the data we pull from the MedList in the ListBox
        public class MedData
        {
            public string Name { get; set; }
            //public string Description { get; set; }
            public ImageSource ImageSource { get; set; }
            public string StartDate { get; set; }

        }
        public MedListOverview()
        {
            InitializeComponent();

            MedData tempMedData = new MedData();

            //Load all medication
            MedImage.Source = tempMedData.ImageSource;
            foreach (Medication item in App.MedList)
            {
                //If there was no photo taken for this Medication show the default picture
                if (item.MedPhoto == null)
                {

                    tempMedData.ImageSource = item.MedPhoto;

                }

                tempMedData.ImageSource = item.MedPhoto;


                //Assign the values from the MedList to the right item that is databound to it
                tempMedData.Name = item.Name;
                tempMedData.StartDate = item.StartDate.ToShortDateString();
                MedListBox.Items.Add(item);

            }



        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri(string.Format("/MainPage.xaml"), UriKind.Relative));
        }

      
        private void MedListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                // If selected index is -1 (no selection) do nothing
                if (MedListBox.SelectedIndex == -1)
                    return;


                ListBox listBox = sender as ListBox;

                if (listBox != null && listBox.SelectedItem != null)
                {
                    //Data hale uit geselecteerde item
                    Medication meddata = (Medication)listBox.SelectedItem;




                    // reset selection of ListBox 
                    ((ListBox)sender).SelectedIndex = -1;

                    //Data doorsture als ge naar de volgende pagina ga
                    FrameworkElement root = Application.Current.RootVisual as FrameworkElement; 
                    root.DataContext = meddata;

                   

                    // change page navigation 
                    NavigationService.Navigate(new Uri(string.Format("/OverviewPage.xaml"), UriKind.Relative));


                    


                }




            }
            
           
        }

    

    }
}
