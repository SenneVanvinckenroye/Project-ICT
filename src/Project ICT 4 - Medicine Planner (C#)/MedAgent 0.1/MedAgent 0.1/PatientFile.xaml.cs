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
<<<<<<< HEAD
        //Helper Class for showing some of the data we pull from the MedList in the ListBox
        public class MedData
        {
            public string Name { get; set; }
            public ImageSource ImageSource { get; set; }
            public string StartDate { get; set; }

            //...

        }

                //Helper Class for showing some of the data we pull from the PatientList in the ListBox
        public class PatientData
        {
            //...
        }

        MedAgent_0_1.MedCareCloudServiceReference.MedPlanServiceClient client;
      
        //Als ge op deze pagina terecht komt kunde checke of den dokter ne patient wil toevoege OF ne patient zen medicijnen wil toevoegen???
=======
>>>>>>> 4b6abd5... Doctor.cs toegevoegd
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string addPatient = "false";
            if (this.NavigationContext.QueryString.TryGetValue("addPatient", out addPatient))
            {
                if (addPatient == "true")
                {
                    PatName.Text = App.PublicPatient.FirstName;
                    StkEdit.Visibility = Visibility.Visible;
                    PatientInfoStackPanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MessageBox.Show("Something went wrong\n\rRedirecting...");
                    NavigationService.GoBack();
                }
            }
            string isPatient = "false";
            if (this.NavigationContext.QueryString.TryGetValue("isPatient", out isPatient))
            {
                if (isPatient == "true")
                {
                    MedAgent_0_1.MedCareCloudServiceReference.MedPlanServiceClient client;
                    client = new MedAgent_0_1.MedCareCloudServiceReference.MedPlanServiceClient();
                    client.GetPatientDataAsync(MainPage.userMemberID);//geef login MemberID door van patient om verdere gegevens op te halen
                    client.GetPatientDataCompleted += new EventHandler<MedAgent_0_1.MedCareCloudServiceReference.GetPatientDataCompletedEventArgs>(client_GetPatientDataCompleted);
                    client.GetPrescriptionsForPatientAsync(App.PublicPatient.Id);
                    client.GetPrescriptionsForPatientCompleted += new EventHandler<MedAgent_0_1.MedCareCloudServiceReference.GetPrescriptionsForPatientCompletedEventArgs>(client_GetPrescriptionsForPatientCompleted);
                }
                else
                {
                    MessageBox.Show("Something went wrong\n\rRedirecting...");
                    NavigationService.GoBack();
                }
            }
            PatName.Text = App.PublicPatient.FirstName + " " + App.PublicPatient.LastName;
        }

        void client_GetPrescriptionsForPatientCompleted(object sender, MedAgent_0_1.MedCareCloudServiceReference.GetPrescriptionsForPatientCompletedEventArgs e)
        {
            if(e.Result != null)
                MedListBox.Items.Add(e.Result);
            else
                MessageBox.Show("Oops, error happened :(\n\rWe couldn't retrieve the Medication List");
        }

        void client_GetPatientDataCompleted(object sender, MedAgent_0_1.MedCareCloudServiceReference.GetPatientDataCompletedEventArgs e)
        {
            if (e.Result.PatientID != -1)
            {
                App.PublicPatient.Id = e.Result.PatientID;
            }
            else
            {
                MessageBox.Show("Something went wrong\n\rRedirecting...");
                NavigationService.GoBack();
            }
        }

        //Constructor
        public PatientFile()
        {
            InitializeComponent();

<<<<<<< HEAD
            //Databinding yo
            PatientInfoStackPanel.DataContext = typeof(Patient);


            //MedData tempMedData = new MedData();

            //Load all medication in the listbox
            foreach (Medication item in App.MedList)
            {
                //If there was no photo taken for this Medication show the default picture
                if (item.MedPhoto == null)
                {

                    //tempMedData.ImageSource = item.MedPhoto;

                }

                //tempMedData.ImageSource = item.MedPhoto;


                //Assign the values from the MedList to the right item that is databound to it
                //tempMedData.Name = item.Name;
                //tempMedData.StartDate = item.StartDate.ToShortDateString();
                MedListBox.Items.Add(item);

            
            Calendar.OnDayClicked += Calendar_OnDayClicked;
=======
>>>>>>> 4b6abd5... Doctor.cs toegevoegd
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

            //Load all medication
            foreach (Medication item in App.MedList)
            {
                MedListBox.Items.Add(item);
            }


            //Panorama Item 1 (MedListOverview)
            ApplicationBar = new ApplicationBar();

            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;

            if (App.UserIsDoctor)
            {
                ApplicationBarIconButton AddMedBtn = new ApplicationBarIconButton();
                AddMedBtn.IconUri = new Uri("/Icons/dark/appbar.add.rest.png", UriKind.RelativeOrAbsolute);
                AddMedBtn.Text = "Add Medication";
                AddMedBtn.Click += ApplicationBarAddButton_OnClick;
                ApplicationBar.Buttons.Add(AddMedBtn);
            }
            ApplicationBarMenuItem EditBtn = new ApplicationBarMenuItem();
            EditBtn.Text = "Edit";
            ApplicationBar.MenuItems.Add(EditBtn);
            EditBtn.Click += ApplicationBarMenuItem_OnClick;

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
<<<<<<< HEAD
            } 
        }

=======

            }
>>>>>>> 4b6abd5... Doctor.cs toegevoegd

        }

        void Calendar_OnDayClicked(object sender, LCalendar.DayClickedEventArgs e)
        {
            Button temp = (Button)sender;
            MessageBox.Show(temp.Tag.ToString());
        }





        //Go to the AddMedPage and create a new Medication object
        private void ApplicationBarAddButton_OnClick(object sender, EventArgs e)
        {

            App.MedList.Add(new Medication());

            NavigationService.Navigate(new Uri(string.Format("/AddMedPage.xaml"), UriKind.Relative));

        }

      
        #endregion

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}