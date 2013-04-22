using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Scheduler;

namespace MedAgent_0_1
{
    public class Medication
    {
        public DateTime Time1 { get; set; }
        public DateTime Time2 { get; set; }
        public DateTime Time3 { get; set; }
        public DateTime Time4 { get; set; }
        public DateTime Time5 { get; set; }
        public DateTime Time6 { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ImageSource MedPhoto { get; set; }

        public int Amount1 { get; set; }
        public int Amount2 { get; set; }
        public int Amount3 { get; set; }
        public int Amount4 { get; set; }
        public int Amount5 { get; set; }
        public int Amount6 { get; set; }


        public Reminder reminder1;
        public Reminder reminder2;
        public Reminder reminder3;
        public Reminder reminder4;
        public Reminder reminder5;
        public Reminder reminder6;
    }
}
