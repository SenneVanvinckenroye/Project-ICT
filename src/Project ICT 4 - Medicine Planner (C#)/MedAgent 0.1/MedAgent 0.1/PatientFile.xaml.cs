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
using System.Xml.Linq;

using MedAgent_0_1;
using System.IO;
using System.IO.IsolatedStorage;

namespace MediAgent
{
    public partial class PatientFile : PhoneApplicationPage
    {
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
                    client = new MedAgent_0_1.MedCareCloudServiceReference.MedPlanServiceClient();
                    client.GetUserDataAsync(MainPage.userMemberID);
                    client.GetUserDataCompleted += new EventHandler<MedAgent_0_1.MedCareCloudServiceReference.GetUserDataCompletedEventArgs>(client_GetUserDataCompleted);
                    client.GetPatientDataAsync(MainPage.userMemberID);//geef login MemberID door van patient om verdere gegevens op te halen
                    client.GetPatientDataCompleted += new EventHandler<MedAgent_0_1.MedCareCloudServiceReference.GetPatientDataCompletedEventArgs>(client_GetPatientDataCompleted);
                }
                else
                {
                    MessageBox.Show("Something went wrong\n\rRedirecting...");
                    NavigationService.GoBack();
                }
            }
        }

        void client_GetUserDataCompleted(object sender, MedAgent_0_1.MedCareCloudServiceReference.GetUserDataCompletedEventArgs e)
        {
            App.PublicPatient.Address = e.Result.address;
            App.PublicPatient.Bday = e.Result.bday;
            App.PublicPatient.Email = e.Result.email;
            App.PublicPatient.FirstName = e.Result.FName;
            App.PublicPatient.LastName = e.Result.LName;
            App.PublicPatient.Sex = e.Result.sex;
            App.PublicPatient.SSN = e.Result.ssn;
            App.PublicPatient.Telephone = e.Result.phoneNumber;

            PatName.Text = App.PublicPatient.LastName;
            PatBday.Text = App.PublicPatient.Bday.ToShortDateString();
            DateTime today = DateTime.Today;
            int age = today.Year - App.PublicPatient.Bday.Year;
            if (App.PublicPatient.Bday > today.AddYears(-age)) age--;
            PatAge.Text = age.ToString();
            PatEmail.Text = App.PublicPatient.Email;
            PatFirstname.Text = App.PublicPatient.FirstName;
            PatSex.Text = App.PublicPatient.Sex.ToString();
            PatSsn.Text = App.PublicPatient.SSN.ToString();
            PatPhone.Text = App.PublicPatient.Telephone.ToString();
        }



        //CHARNAARBOOL WEG WANT TAKEN IS NU REEDS BOOLEAN

        void client_GetPatientDataCompleted(object sender, MedAgent_0_1.MedCareCloudServiceReference.GetPatientDataCompletedEventArgs e)
        {

            if (e.Result.PatientID != -1)
            {
                client = new MedAgent_0_1.MedCareCloudServiceReference.MedPlanServiceClient();
                App.PublicPatient.Id = e.Result.PatientID;


                client.GetPrescriptionsForPatientAsync(Convert.ToInt32(App.PublicPatient.Id));
                client.GetPrescriptionsForPatientCompleted += new EventHandler<MedAgent_0_1.MedCareCloudServiceReference.GetPrescriptionsForPatientCompletedEventArgs>(client_GetPrescriptionsForPatientCompleted);
            }
            else
            {
                MessageBox.Show("Something went wrong\n\rRedirecting...");
                NavigationService.GoBack();
            }

        }

        void client_GetPrescriptionsForPatientCompleted(object sender, MedAgent_0_1.MedCareCloudServiceReference.GetPrescriptionsForPatientCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                App.MedList.Clear();
                foreach (var item in e.Result)
                {
                    App.PrescriptionsCounter = item.PrescriptionID;
                    MedListBox.Items.Clear();
                    Medication tempMed = new Medication();
                    string xmlData = item.data;
                    XDocument xml = XDocument.Parse(item.data);

                    string xmlFileName = "Prescription" + item.PrescriptionID + ".xml";//ZO KRIJGEN WE LOKALE XML BESTANDEN VOOR ELK VOORSCHRIFT
                    //xml.Load(); // suppose that myXmlString contains "<Names>...</Names>"
                    //SLA HET XML BESTAND LOKAAL OP
                    using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (IsolatedStorageFileStream isoStream =
                            new IsolatedStorageFileStream(xmlFileName, FileMode.Create, isoStore))
                        {
                            xml.Save(isoStream);
                        }
                    }

                    var prescription = from p in xml.Descendants("Prescription") select p;///???

                    tempMed.Name = prescription.Elements("DrugName").First().Value;
                    tempMed.Description = prescription.Elements("DrugDescription").First().Value;
                    tempMed.Amount = Convert.ToInt32(prescription.Elements("Quantity").First().Value);
                    tempMed.StartDate = Convert.ToDateTime(prescription.Elements("StartDate").First().Value);
                    tempMed.EndDate = Convert.ToDateTime(prescription.Elements("EndDate").First().Value);

                    tempMed.Days = new List<Day>();

                    foreach (var xElement in prescription.Elements("Days").Elements())
                    {
                        Day TempDay = new Day();
                        TempDay.Time = new List<TimeSpan>();
                        TempDay.Taken = new List<bool>();
                        TempDay.Administration = new List<string>();

                        DateTime TempDate;
                        DateTime.TryParse(xElement.Element("Date").Value, out TempDate);
                        TempDay.Date = TempDate;

                        foreach (var element in xElement.Elements())
                        {
                            if (element.DescendantsAndSelf("Time").Any())
                            {
                                TimeSpan tempTime;
                                TimeSpan.TryParse(element.Elements("Time").First().Value, out tempTime);
                                TempDay.Time.Add(tempTime);
                            }
                            if (element.DescendantsAndSelf("Taken").Any())
                            {
                                bool tempTaken;
                                bool.TryParse(element.Elements("Taken").First().Value, out tempTaken);
                                TempDay.Taken.Add(tempTaken);
                            }
                            if (element.DescendantsAndSelf("Administration").Any())
                            {
                                TempDay.Administration.Add(element.Elements("Administration").First().Value);
                            }

                        }
                        tempMed.Days.Add(TempDay);
                    }



                    //COURSE IS NU INTEGER DUS '1' IS EVERY DAY ETC....
                    //tempMed.Administration = "2 tablets";
                    /*string courseInString;
                    switch (Convert.ToInt32(prescription.Elements("Course").First().Value))
                    {
                        case 1:
                            {
                                courseInString = "Daily";
                                break;
                            }
                        default:
                            break;
                            
                    }*/
                    int CourseOut;
                    int.TryParse(prescription.Elements("Course").First().Value, out CourseOut);
                    tempMed.Interval = CourseOut;
                    /*tempMed.Times[0] = item.Time1;
                    tempMed.Times[1] = item.Time2;
                    tempMed.Times[2] = item.Time3;
                    tempMed.Times[3] = item.Time4;
                    tempMed.Times[4] = item.Time5;
                    tempMed.Times[5] = item.Time6;*/

                    /*tempMed.Taken[0][0] = CharNaarBool(item.Taken1);
                    tempMed.Taken[0][1] = CharNaarBool(item.Taken2);
                    tempMed.Taken[0][2] = CharNaarBool(item.Taken3);
                    tempMed.Taken[0][3] = CharNaarBool(item.Taken4);
                    tempMed.Taken[0][4] = CharNaarBool(item.Taken5);
                    tempMed.Taken[0][5] = CharNaarBool(item.Taken6);*/

                    App.MedList.Add(tempMed);
                }
                foreach (Medication MedItem in App.MedList)
                {
                    MedListBox.Items.Add(MedItem);
                }
                LCalendar MedCal = new LCalendar();
                MedCal.Name = "Calendar";
                MedCal.OnDayClicked += Calendar_OnDayClicked;
                


                KalenderPanoItem.Children.Add(MedCal);

            }
            else
                MessageBox.Show("Couldn't retrieve any medication.");
        }

        //Constructor
        public PatientFile()
        {
            InitializeComponent();

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


                //Calendar.OnDayClicked += Calendar_OnDayClicked;
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

    }
}