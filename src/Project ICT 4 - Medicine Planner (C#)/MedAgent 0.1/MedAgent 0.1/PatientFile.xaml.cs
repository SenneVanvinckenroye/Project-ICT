using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MedAgent_0_1;

namespace MediAgent
{
    public partial class PatientFile : PhoneApplicationPage
    {
        public PatientFile()
        {
            InitializeComponent();

            PatName.Text = MainPage.PublicPatient.FirstName + " " + MainPage.PublicPatient.LastName;
            PatAge.Text = MainPage.PublicPatient.bDay != new DateTime() ? DateTime.Now.Date.Subtract(MainPage.PublicPatient.bDay.Date).ToString() : "NA";
            PatSex.Text = MainPage.PublicPatient.Sex != '\0' ? MainPage.PublicPatient.Sex.ToString() : "NA";
            PatEmail.Text = MainPage.PublicPatient.Email ?? "NA";
            PatBday.Text = MainPage.PublicPatient.bDay != new DateTime() ? MainPage.PublicPatient.bDay.Date.ToString() : "NA";
            PatSsn.Text = MainPage.PublicPatient.Ssn != 0 ? MainPage.PublicPatient.Ssn.ToString() : "NA";

            PatNameEdit.Text = PatName.Text;
            PatAgeEdit.Text = PatAge.Text;
            PatSexEdit.Text = PatSex.Text;
            PatEmailEdit.Text = PatEmail.Text;
            PatBdayEdit.Text = PatBday.Text;
            PatSsnEdit.Text = PatSsn.Text;

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
    }
}