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
using MedAgent_0_1;
using Microsoft.Phone.Controls;

namespace MediAgent
{
    public partial class DoctorView1 : PhoneApplicationPage
    {
        public List<Patient> TestList;
        MedAgent_0_1.MedCareCloudServiceReference.MedPlanServiceClient client;
        public DoctorView1()
        {
            InitializeComponent();
            //Service.DownloadDone += Service_DownloadDone;
            //Service.Query(null, "lastName");
            client = new MedAgent_0_1.MedCareCloudServiceReference.MedPlanServiceClient();

            client.GetAllPatientsForDoctorAsync(1);//change 1 to DocID from login
            client.GetAllPatientsForDoctorCompleted += new EventHandler<MedAgent_0_1.MedCareCloudServiceReference.GetAllPatientsForDoctorCompletedEventArgs>(client_GetAllPatientsCompleted);

        }

        void client_GetAllPatientsCompleted(object sender, MedAgent_0_1.MedCareCloudServiceReference.GetAllPatientsForDoctorCompletedEventArgs e)
        {
            Patient[] test = new Patient[e.Result.Count];
            for (int i = 0; i < e.Result.Count; i++)
            {
                test[i] = new Patient();
                test[i].FirstName = e.Result[i].FirstName;
                test[i].LastName = e.Result[i].LastName;
                test[i].Id = e.Result[i].PatientID;
                test[i].Email = e.Result[i].Email;
                test[i].Bday = e.Result[i].bDay;
                test[i].Sex = e.Result[i].Sex;
                test[i].SSN = e.Result[i].Ssn;

            }
            foreach (Patient patient in test)
            {
                PatientLst.Items.Add(patient);
            }
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
            //
            if (PatientLst.SelectedItem != null && PatientLst.SelectedItem.ToString() == "    Patient's file")
            {
                //App.PublicPatient = PatientLst.Items.ElementAt(PatientLst.SelectedIndex - 2) as Patient;
                //PatientLst.SelectedIndex = PatientLst.SelectedIndex - 2;
                NavigationService.Navigate(new Uri("/PatientFile.xaml", UriKind.Relative));
            }
            else if (PatientLst.SelectedItem != null && PatientLst.SelectedItem.ToString() == "    Course of medication")
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
                PatientLst.Items.Insert(PatientLst.SelectedIndex + 1, "    Course of medication");
            }

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
                NavigationService.Navigate(new Uri("/PatientFile.xaml?addPatient=true", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("Wrong input detected!");
            }

        }
    }
}