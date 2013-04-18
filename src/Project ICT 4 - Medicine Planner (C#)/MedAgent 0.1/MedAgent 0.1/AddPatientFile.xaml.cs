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
using System.Windows.Navigation;
using Microsoft.Phone.Tasks;

namespace MedAgent_0_1
{
    public partial class AddPatientFile : PhoneApplicationPage
    {
        MedAgent_0_1.MedCareCloudServiceReference.MedPlanServiceClient client;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string addPatient = "false";
            if (this.NavigationContext.QueryString.TryGetValue("addPatient", out addPatient))
            {
                if (addPatient == "true")
                {
                    PatNameEdit.Text = App.PublicPatient.LastName;
                    PatFirstnameEdit.Text = App.PublicPatient.FirstName;
                    PatEmailEdit.Text = App.PublicPatient.Email;
                }
                else
                {
                    MessageBox.Show("Something went wrong\n\rRedirecting...");
                    NavigationService.GoBack();
                }
            }

        }



        public AddPatientFile()
        {
            InitializeComponent();
        }

        private void AddPatientMenuItem_OnClick(object sender, EventArgs e)
        {
            //validate fields, send patient to db
            MessageBox.Show("Make sure all fields are correct!");
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            App.PublicPatient.FirstName = PatFirstnameEdit.Text;
            App.PublicPatient.LastName = PatNameEdit.Text;
            App.PublicPatient.Email = PatEmailEdit.Text;
            //data needed:
            //current docter ID

            //generated password
            PasswordGenerator pGen = new PasswordGenerator();
            string randomPass = pGen.GenPassWithCap(8);
            //email paswoord en hashen naar database
            sendMail(App.PublicPatient.Email,App.PublicPatient.FirstName,randomPass);
            //FName
            //LName
            //email
            //
        }
        //bool mailstatus;
        private void sendMail(string email,string FName,string randomPass)
        {
            client = new MedAgent_0_1.MedCareCloudServiceReference.MedPlanServiceClient();
            client.SendEmailAsync(email, FName, "Van Beek", randomPass);
            client.SendEmailCompleted += new EventHandler<MedCareCloudServiceReference.SendEmailCompletedEventArgs>(client_SendEmailCompleted);
        }

        void client_SendEmailCompleted(object sender, MedCareCloudServiceReference.SendEmailCompletedEventArgs e)
        {
            MessageBox.Show("Email to patient send");
            /*if (e.Result == true)//returned true? / succesfull email?
            {
                MessageBox.Show("Email to patient send");
            }
            else
            {
                MessageBox.Show("Failed to send email to patient!");
            }*/
        }

    }
}