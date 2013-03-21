using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MediAgent
{
    public partial class PatientFile : PhoneApplicationPage
    {
        public PatientFile()
        {
            InitializeComponent();
            //PatName.Text = App.Pat.FirstName + " " + App.Pat.LastName;
        }
    }
}