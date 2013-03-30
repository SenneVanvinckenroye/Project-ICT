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
                PatientLst.Items.Add(new Patient() { FirstName = item.FirstName, LastName = item.LastName, Id = item.PatientID });
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
            for (int i = 0; i < PatientLst.Items.Count; i++)
            {
                if (!(PatientLst.Items.ElementAt(i) is Patient) && (PatientLst.SelectedItem is Patient))
                {
                    PatientLst.Items.RemoveAt(i);
                    PatientLst.Items.RemoveAt(i);
                }
            }
            if (PatientLst.SelectedItem != null && PatientLst.SelectedItem.ToString() == "    Patient's file")
            {
                //App.Pat = PatientLst.Items.ElementAt(PatientLst.SelectedIndex - 2) as Patient;
                NavigationService.Navigate(new Uri("/PatientFile.xaml", UriKind.Relative));
            }
            if (PatientLst.SelectedItem is Patient)
            {
                PatientLst.Items.Insert(PatientLst.SelectedIndex + 1, "    Patient's file");
                PatientLst.Items.Insert(PatientLst.SelectedIndex + 1, "    Course of medication");
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PatientLst.Items.Clear();
            //foreach (Patient patient in Service.Load())
            //{
            //    PatientLst.Items.Add(patient);
            //}
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int temp;
            if (TxtboxFirstName.Text.Length >= 2 && TxtboxLastName.Text.Length >= 2 && TxtboxTelephone.Text.Length >= 9 && TxtboxTelephone.Text.Length <= 10 && TxtboxAdres.Text.Length >= 2 && Int32.TryParse(TxtboxTelephone.Text,out temp))
            {
                //Service.Insert(TxtboxFirstName.Text, TxtboxLastName.Text, temp, TxtboxAdres.Text);
            }
            
        }
    }

    public class Patient
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Id { get; set; }
        public int Telephone { get; set; }
        public string Address { get; set; }
        public string MedicineHistory { get; set; }
        public List<string> Symptoms { get; set; }

        public override string ToString()
        {
            return LastName + ", " + FirstName + ",  Id: " + Id;
        }
    }
}