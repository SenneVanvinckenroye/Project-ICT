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
        public TimeSpan[] Times { get; set; }

        private TimeSpan DefaultTime = new TimeSpan();

        public Medication()
        {
            //When the times aren't set they will be given the default value
            Times = new TimeSpan[]
                {
                    DefaultTime,DefaultTime,DefaultTime,DefaultTime,DefaultTime,DefaultTime
                };
        }
        //public string Administration { get; set; }

        //public List<List<bool>> Taken { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Interval { get; set; }

        public ImageSource MedPhoto { get; set; }

        public int Course { get; set; }

        public int Amount { get; set; }

        public Reminder[] Reminders { get; set; }

        public List<Day> Days { get; set; }
    }

    public class Day
    {
        public DateTime Date { get; set; }

        public List<TimeSpan> Time { get; set; }

        public List<bool> Taken { get; set; }

        public List<String> Administration { get; set; }
    }
}
