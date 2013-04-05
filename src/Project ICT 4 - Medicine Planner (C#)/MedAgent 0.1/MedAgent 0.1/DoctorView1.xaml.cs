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

            client.GetAllPatientsForDocterAsync(1);//change 1 to DocID from login
            client.GetAllPatientsForDocterCompleted += new EventHandler<MedAgent_0_1.MedCareCloudServiceReference.GetAllPatientsForDocterCompletedEventArgs>(client_GetAllPatientsCompleted);
            
        }

        void client_GetAllPatientsCompleted(object sender, MedAgent_0_1.MedCareCloudServiceReference.GetAllPatientsForDocterCompletedEventArgs e)
        {
            foreach (var item in e.Result)
            {
                PatientLst.Items.Add(new Patient() { FirstName = item.FirstName, LastName = item.LastName, Id = item.PatientID});
            }
        }

        void Service_DownloadDone(object sender, EventArgs e)
        {
            PatientLst.Items.Clear();
            //foreach (Patient patient in Service.Load())
            //{
            //    PatientLst.Items.Add(patient);
            //}
        }


        private void PatientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (PatientLst.SelectedItem != null && PatientLst.SelectedItem.ToString() == "    Patient's file")
            {
                MainPage.PublicPatient = PatientLst.Items.ElementAt(PatientLst.SelectedIndex - 2) as Patient;
                //PatientLst.SelectedIndex = PatientLst.SelectedIndex - 2;
                NavigationService.Navigate(new Uri("/PatientFile.xaml", UriKind.Relative));
            }
            else if (PatientLst.SelectedItem != null && PatientLst.SelectedItem.ToString() == "    Course of medication")
            {
                //App.Pat = PatientLst.Items.ElementAt(PatientLst.SelectedIndex - 2) as Patient;
                MainPage.PublicPatient = PatientLst.Items.ElementAt(PatientLst.SelectedIndex - 1) as Patient;
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
                PatientLst.Items.Insert(PatientLst.SelectedIndex + 1, "    Patient's file");
                PatientLst.Items.Insert(PatientLst.SelectedIndex + 1, "    Course of medication");
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (TxtboxFirstName.Text.Length >= 2 && TxtboxLastName.Text.Length >= 2 && TxtboxEmail.Text.Length >= 2 && TxtboxEmail.Text.Length <= 50) // input check
            {
                //Service.Insert(TxtboxFirstName.Text, TxtboxLastName.Text, temp, TxtboxAdres.Text);
            }
            
        }
    }
}