using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using CalendarControl;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Shapes;
using Microsoft.Phone.Tasks;
using Microsoft.Phone;

using MedAgent_0_1;

namespace MediAgent
{
    public partial class PatientFile : PhoneApplicationPage
    {
        //Helper Class for showing some of the data we pull from the MedList in the ListBox
        public class MedData
        {
            public string Name { get; set; }
            //public string Description { get; set; }
            public ImageSource ImageSource { get; set; }
            public string StartDate { get; set; }

        }
      
        //WTF DOET DEES???
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string addPatient = "false";
            if (this.NavigationContext.QueryString.TryGetValue("addPatient", out addPatient))
            {
                if (addPatient == "true")
                {
                    PatName.Text = App.PublicPatient.FirstName;
                    StkEdit.Visibility = Visibility.Visible;
                    StkNoEdit.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MessageBox.Show("Something went wrong\n\rRedirecting...");
                    NavigationService.GoBack();
                }
            }

            PatName.Text = App.PublicPatient.FirstName + " " + App.PublicPatient.LastName;
        }

        //Constructor
        public PatientFile()
        {
            InitializeComponent();


            MedData tempMedData = new MedData();

            //Load all medication
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

            
            Calendar.OnDayClicked += Calendar_OnDayClicked;
            PatName.Text = App.PublicPatient.LastName;
            PatFirstname.Text = App.PublicPatient.FirstName;

            if (App.PublicPatient.Bday != new DateTime())
            {
                int age = DateTime.Today.Year - App.PublicPatient.Bday.Year;
                if (App.PublicPatient.Bday > DateTime.Today.AddYears(-age)) age--;
                PatAge.Text = age.ToString();
            }
            else
            {
                PatAge.Text = "NA";
            }

            PatSex.Text = App.PublicPatient.Sex != '\0' ? App.PublicPatient.Sex.ToString() : "NA";
            PatEmail.Text = App.PublicPatient.Email ?? "NA";
            PatBday.Text = App.PublicPatient.Bday != new DateTime() ? App.PublicPatient.Bday.Date.ToString() : "NA";
            PatSsn.Text = App.PublicPatient.SSN != 0 ? App.PublicPatient.SSN.ToString() : "NA";

            PatNameEdit.Text = PatName.Text;
            PatFirstnameEdit.Text = PatFirstname.Text;
            PatAgeEdit.Text = PatAge.Text;
            PatSexEdit.Text = PatSex.Text;
            PatEmailEdit.Text = PatEmail.Text;
            PatBdayEdit.Text = PatBday.Text;
            PatSsnEdit.Text = PatSsn.Text;
        }


        //Panorama Item 1 (MedListOverview)




        }



        //All events
        #region
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



        void Calendar_OnDayClicked(object sender, LCalendar.DayClickedEventArgs e)
        {
            Button temp = (Button) sender;
            MessageBox.Show(temp.Tag.ToString());
        }

        private void ApplicationBarMenuItem_OnClick(object sender, EventArgs e)
        {
            if (StkEdit.Visibility == Visibility.Visible)
            {
                StkNoEdit.Visibility = Visibility.Visible;
                StkEdit.Visibility = Visibility.Collapsed;
            }
            else
            {
                StkNoEdit.Visibility = Visibility.Collapsed;
                StkEdit.Visibility = Visibility.Visible;
            }
        }







        //Go to the AddMedPage and create a new Medication object
        private void ApplicationBarAddButton_OnClick(object sender, EventArgs e)
        {

            App.MedList.Add(new Medication());

            NavigationService.Navigate(new Uri(string.Format("/AddMedPage.xaml"), UriKind.Relative));

        }

       
        private void ApplicationBarDeleteButton_OnClick(object sender, EventArgs e)
        {

        }
        #endregion

    }
}