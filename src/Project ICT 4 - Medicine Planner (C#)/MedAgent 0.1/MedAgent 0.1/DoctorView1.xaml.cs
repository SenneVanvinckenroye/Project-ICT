﻿using System;
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
using MedAgent_0_1;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace MediAgent
{


    public partial class DoctorView1 : PhoneApplicationPage
    {

        //Field _currentItemIndex will be used as an index field for the currently selected patient
        private int _currentItemIndex = -1;

        //This property will implement our entire selection logic so when a patient object is set it will be selected as current.
        private Patient CurrentPatient
        {
            get
            {
                if (_currentItemIndex != -1)
                    return this.TestList[_currentItemIndex];
                else
                {
                    return null;
                }
            }

            set 
            { 
                Patient patient = value; 
                if (patient != null)
                {
                    _currentItemIndex = TestList.IndexOf(patient);
                    PatientLst.DataContext = patient;
                }
                else
                {
                    PatientLst.DataContext = typeof (Patient);
                    _currentItemIndex = -1;
                }
            }
        }



        public List<Patient> TestList;

        MedAgent_0_1.MedCareCloudServiceReference.MedPlanServiceClient client;

        //Constructor
        public DoctorView1()
        {
            InitializeComponent();

            //Load all patients in the listbox

            
            foreach (Patient item in App.PatList)
            {
                //PatientLst.Items.Add(item);
            }

            //Service.DownloadDone += Service_DownloadDone;
            //Service.Query(null, "lastName");
            client = new MedAgent_0_1.MedCareCloudServiceReference.MedPlanServiceClient();

            //Databinding yo
            //PatientLst.DataContext = typeof (Patient);

            client.GetAllPatientsForDoctorAsync(Convert.ToInt32(App.PublicDoctor.DocID));//change 1 to DocID from login
            client.GetAllPatientsForDoctorCompleted += new EventHandler<MedAgent_0_1.MedCareCloudServiceReference.GetAllPatientsForDoctorCompletedEventArgs>(client_GetAllPatientsForDoctorCompleted);

        }

        
        void client_GetAllPatientsForDoctorCompleted(object sender, MedAgent_0_1.MedCareCloudServiceReference.GetAllPatientsForDoctorCompletedEventArgs e)
        {
            //MessageBox.Show(e.Result);
            if (e.Result != null)
            {
                if (e.Result.Count > 0)
                {
                    Patient[] test = new Patient[e.Result.Count];
                    for (int i = 0; i < e.Result.Count; i++)
                    {

                        //We zette de database results in de publieke PatList 
                        test[i] = new Patient();
                        test[i].FirstName = e.Result[i].FirstName;
                        test[i].LastName = e.Result[i].LastName;
                        test[i].Id = e.Result[i].PatientID;
                        test[i].Email = e.Result[i].Email;
                        test[i].Bday = e.Result[i].bDay;
                        test[i].Sex = e.Result[i].Sex;
                        test[i].SSN = e.Result[i].Ssn;
                        test[i].Address = e.Result[i].address;
                        test[i].Telephone = e.Result[i].phoneNumber.ToString();

                    }
                    foreach (Patient patient in test)
                    {
                        App.PatID++;
                        PatientLst.Items.Add(patient);
                        App.PatList.Add(patient);
                    }
                }
                else
                {
                    MessageBox.Show("Server error :(\n\rWe could not find any of your patients");
                }
            }
            else
                MessageBox.Show("Server error :(\n\rWe could not return a patientlist");
        }

        //void Service_DownloadDone(object sender, EventArgs e)
        //{
        //    PatientLst.Items.Clear();
        //foreach (Patient patient in Service.Load())
        //{
        //    PatientLst.Items.Add(patient);
        //}
        //}


        private void PatientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                
                // If selected index  is -1 (no selection) do nothing
                if (PatientLst.SelectedIndex == -1)
                    return;


                ListBox listBox = sender as ListBox;

                if (listBox != null && listBox.SelectedItem != null)
                {
                    //Data hale uit geselecteerde item
                    Patient patdata = (Patient)listBox.SelectedItem;


                    // reset selection of ListBox 
                    ((ListBox)sender).SelectedIndex = -1;

                    //Data doorsture als ge naar de volgende pagina ga
                    FrameworkElement root = Application.Current.RootVisual as FrameworkElement; 
                    
                    root.DataContext = patdata;

                    App.PublicPatient.Id = patdata.Id;
                    App.PublicPatient.Address = patdata.Address;
                    App.PublicPatient.Bday = patdata.Bday;
                    App.PublicPatient.Email = patdata.Email;
                    App.PublicPatient.FirstName = patdata.FirstName;
                    App.PublicPatient.LastName = patdata.LastName;
                    App.PublicPatient.Sex = patdata.Sex;
                    App.PublicPatient.SSN = patdata.SSN;
                    App.PublicPatient.Telephone = patdata.Telephone;
                    /*App.PublicPatient.MedicineHistory = "";
                    App.PublicPatient.Symptoms = new List<string>();*/

                    // change page navigation 
                    NavigationService.Navigate(new Uri(string.Format("/PatientFile.xaml?isDoctor=true"), UriKind.Relative));

                }


            }


            //Dit doen we nimeer
                /*
            else if (PatientLst.SelectedItem != null && PatientLst.SelectedItem.ToString() == "    Add medication")
            {
                //App.Pat = PatientLst.Items.ElementAt(PatientLst.SelectedIndex - 2) as Patient;
                //App.PublicPatient = PatientLst.Items.ElementAt(PatientLst.SelectedIndex - 1) as Patient;
                NavigationService.Navigate(new Uri("/AddMedPage.xaml", UriKind.Relative));
            }

            while (!(PatientLst.SelectedItem is Patient))
            {
                PatientLst.SelectedIndex--;
            }

            for (int i = 0; i < PatientLst.Items.Count; i++)
            {
                if (!(PatientLst.Items.ElementAt(i) is Patient))
                {
                    PatientLst.Items.RemoveAt(i);
                    PatientLst.Items.RemoveAt(i);
                }
            }

            if (PatientLst.SelectedItem is Patient)
            {
                App.PublicPatient = (Patient)PatientLst.SelectedItem;
                PatientLst.Items.Insert(PatientLst.SelectedIndex + 1, "    Patient's file");
                PatientLst.Items.Insert(PatientLst.SelectedIndex + 1, "    Add medication");
            }*/

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            bool EmailIsValid = Validator.EmailIsValid(TxtboxEmail.Text);
            if (TxtboxFirstName.Text.Length >= 2 && TxtboxLastName.Text.Length >= 2 && EmailIsValid) // input check
            {
                //Service.Insert(TxtboxFirstName.Text, TxtboxLastName.Text, temp, TxtboxAdres.Text);
                App.PublicPatient.FirstName = TxtboxFirstName.Text;
                App.PublicPatient.LastName = TxtboxLastName.Text;
                App.PublicPatient.Email = TxtboxEmail.Text;
                NavigationService.Navigate(new Uri("/AddPatientFile.xaml?addPatient=true", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("Wrong input detected!");
            }

        }

        //private void PhonesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //Call the patient his number, VETTEN DJAB

        //}

        //Go to the AddPatientPage and create a new Patient object
        private void ApplicationBarAddButton_OnClick(object sender, EventArgs e)
        {
            App.PatList.Add(new Patient());

            NavigationService.Navigate(new Uri(string.Format("/AddPatientFile.xaml"), UriKind.Relative));
        }

        private void PhonePatBtn_OnClick(object sender, RoutedEventArgs e)
        {
            Button test = (Button) e.OriginalSource;
            int listID = Convert.ToInt32(test.DataContext)-1;//patientID -1
            PhoneCallTask callTask = new PhoneCallTask();
            callTask.PhoneNumber = App.PatList[listID].Telephone;
            callTask.DisplayName = App.PatList[listID].FirstName;
            callTask.Show();
            //Call the patient his number, VETTEN DJAB
        }
    }
}