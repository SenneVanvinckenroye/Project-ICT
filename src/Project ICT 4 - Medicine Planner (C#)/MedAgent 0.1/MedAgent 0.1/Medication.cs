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

        private int _interval;

        public int Interval {
            get
            {
                return _interval;
            }

            set
            {
                switch (value)
                {
                    case 1:
                        {
                            Course = "Every day";
                            break;
                        }
                    case 2:
                        {
                            Course = "Every 2 days";
                            break;
                        }
                    case 3:
                        {
                            Course = "Every 3 days";
                            break;
                        }
                    case 4:
                        {
                            Course = "Every 4 days";
                            break;
                        }
                    case 5:
                        {
                            Course = "Every 5 days";
                            break;
                        }
                    case 6:
                        {
                            Course = "Every 6 days";
                            break;
                        }
                    case 7:
                        {
                            Course = "Weekly";
                            break;
                        }
                }
                _interval = value;
            }
        }

        public ImageSource MedPhoto { get; set; }

        public string Course { get; set; }

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

    public class DayMedListClass
    {
        public DateTime Date { get; set; }
        public String Name { get; set; }
        public TimeSpan Time { get; set; }
        public Medication Medication { get; set; }
    }
}
