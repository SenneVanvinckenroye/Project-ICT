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
            //PatName.Text = App.Pat.FirstName + " " + App.Pat.LastName;
            PatName.Text = MainPage.userFName+" "+MainPage.userLName;
        }
    }
}