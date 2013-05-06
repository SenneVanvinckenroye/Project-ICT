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


        /*
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

        }*/



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
            App.PatList[App.PatID].FirstName = PatFirstnameEdit.Text;
            App.PatList[App.PatID].LastName = PatNameEdit.Text;
            App.PatList[App.PatID].Email = PatEmailEdit.Text;
            App.PatList[App.PatID].Sex = PatSexEdit.Text.ToCharArray().FirstOrDefault();//field male/female -> m/f

            DateTime MyDateTime;
            MyDateTime = new DateTime();

            MyDateTime = (DateTime)DateOfBirth.Value;


            //data needed:
            //current docter ID

            //generated password
            PasswordGenerator pGen = new PasswordGenerator();
            string randomPass = pGen.GenPassWithCap(8);
            //email paswoord en hashen naar database
            //sendMail(App.PublicPatient.Email,App.PublicPatient.FirstName,randomPass);
            string randomPassHash = MD5Core.GetHashString(randomPass);
            randomPass = null;
            
            client = new MedAgent_0_1.MedCareCloudServiceReference.MedPlanServiceClient();
            //Checke of alle velde legit zijn
            int SSN;
            bool isNum = Int32.TryParse(PatSsnEdit.Text,out SSN);

            // er moeten meer checks in deze "if"... (phoneNum checken of het een nummer is)
            if (PatNameEdit.Text != "" && PatFirstnameEdit.Text != "" && (PatSexEdit.Text.ToCharArray().First() == 'm' || PatSexEdit.Text.ToCharArray().First() == 'f') && Validator.EmailIsValid(PatEmailEdit.Text) && (PatSsnEdit.Text!="" && isNum) )
            {
                 client.CreateNewUserAsync(PatFirstnameEdit.Text, PatNameEdit.Text, randomPassHash, PatEmailEdit.Text, PatSexEdit.Text.ToCharArray().FirstOrDefault(), 1, 'p', MyDateTime, PatAddress.Text, Convert.ToInt32(PatSsnEdit.Text), App.PublicDoctor.DocID, Convert.ToInt32(PatPhoneNumber.Text));
            }

            else
            {
                MessageBox.Show("Not all fields have been filled in correctly");
            }
           
            client.CreateNewUserCompleted += new EventHandler<MedCareCloudServiceReference.CreateNewUserCompletedEventArgs>(client_CreateNewUserCompleted);
        }

        void client_CreateNewUserCompleted(object sender, MedCareCloudServiceReference.CreateNewUserCompletedEventArgs e)
        {
            //If shit is succesfull, go yolooo
            if (e.Result == "k")
            {
                MessageBox.Show("User succesfully added to db");

                App.PatID++;

                NavigationService.Navigate(new Uri(string.Format("/DoctorView1.xaml"), UriKind.Relative));
            }

            else
                MessageBox.Show("Failed to add user to db\n\rError: "+e.Result);




        }

        private void DateOfBirth_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            
        }

        //bool mailstatus;
        /*
        private void sendMail(string email,string FName,string randomPass)
        {
            client = new MedAgent_0_1.MedCareCloudServiceReference.MedPlanServiceClient();
            client.SendEmailAsync(email, FName, "Van Beek", randomPass);
            client.SendEmailCompleted += new EventHandler<MedCareCloudServiceReference.SendEmailCompletedEventArgs>(client_SendEmailCompleted);
        }

        void client_SendEmailCompleted(object sender, MedCareCloudServiceReference.SendEmailCompletedEventArgs e)
        {
            
            if (e.Result == true)//returned true? / succesfull email?
            {
                MessageBox.Show("Email to patient send");
            }
            else
            {
                MessageBox.Show("Failed to send email to patient!");
            }
        }
        */
    }
}