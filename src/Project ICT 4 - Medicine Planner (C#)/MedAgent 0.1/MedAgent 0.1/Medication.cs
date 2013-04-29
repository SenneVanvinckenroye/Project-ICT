﻿using System;
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


        public DateTime[] Times { get; set; }
        

        private DateTime DefaultTime = new DateTime(1600,1,1);

        public Medication()
        {
            //When the times aren't set they will be given the default value
            Times = new DateTime[]
                {
                    DefaultTime,DefaultTime,DefaultTime,DefaultTime,DefaultTime,DefaultTime
                };

 
        }


        public List<List<bool>> Taken { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int Interval { get; set; }

        public ImageSource MedPhoto { get; set; }

        public string Course { get; set; }

        public int Amount { get; set; }

        public Reminder[] Reminders { get; set; }
    }
}
