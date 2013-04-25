using System;
using System.Collections.Generic;
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
        public DateTime[] Time { get; set; }

        public List<List<bool>> Taken { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int Interval { get; set; }

        public ImageSource MedPhoto { get; set; }

<<<<<<< HEAD
        public string Course { get; set; }
=======
        public int[] Amount { get; set; }
>>>>>>> 2a21007... Kalender is af

        public Reminder[] Reminder { get; set; }
    }
}
