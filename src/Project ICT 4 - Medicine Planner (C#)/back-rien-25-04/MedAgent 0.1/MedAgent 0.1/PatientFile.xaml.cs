﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using CalendarControl;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MedAgent_0_1;

namespace MediAgent
{
    public partial class PatientFile : PhoneApplicationPage
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string addPatient = "false";
            if (this.NavigationContext.QueryString.TryGetValue("addPatient", out addPatient))
            {
                if (addPatient == "true")
                {
                    PatName.Text = App.PublicPatient.FirstName;
                    StkEdit.Visibility = Visibility.Visible;
                    StkNoEdit.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MessageBox.Show("Something went wrong\n\rRedirecting...");
                    NavigationService.GoBack();
                }
            }

            PatName.Text = App.PublicPatient.FirstName + " " + App.PublicPatient.LastName;
        }
        public PatientFile()
        {
            InitializeComponent();
            Calendar.OnDayClicked += Calendar_OnDayClicked;
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

        void Calendar_OnDayClicked(object sender, LCalendar.DayClickedEventArgs e)
        {
            Button temp = (Button) sender;
            MessageBox.Show(temp.Tag.ToString());
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