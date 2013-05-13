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
        MedAgent_0_1.MedCareCloudServiceReference.MedPlanServiceClient client;
        public OverviewPage()
        {

            InitializeComponent();

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.GoBack(); 

        }
        int PrescriptionID = 0;
        private void RemoveMed_Click(object sender, RoutedEventArgs e)
        {
            client = new MedCareCloudServiceReference.MedPlanServiceClient();
            PrescriptionID = Convert.ToInt32(PrescriptionIDTextBlock.Text);
            client.DropMedAsync(PrescriptionID);
            client.DropMedCompleted += new EventHandler<MedCareCloudServiceReference.DropMedCompletedEventArgs>(client_DropMedCompleted);
        }

        void client_DropMedCompleted(object sender, MedCareCloudServiceReference.DropMedCompletedEventArgs e)
        {
            //App.MedList.Remove(App.MedList[App.MedID]);
            for (int i = 0; i < App.MedList.Count; i++)
            {
                if (App.MedList[i].PrescriptionID == PrescriptionID)
                {
                    App.MedList.RemoveAt(i);
                }
            }
            MessageBox.Show("Medication deleted");
            NavigationService.GoBack();
            NavigationService.Navigate(new Uri("/PatientFile.xaml"));
        }
    }
}
