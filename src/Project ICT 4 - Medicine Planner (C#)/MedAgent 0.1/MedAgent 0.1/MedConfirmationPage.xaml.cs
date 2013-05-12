using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MedAgent_0_1
{
    public partial class MedConfirmationPage : PhoneApplicationPage
    {
        public MedConfirmationPage()
        {
            InitializeComponent();

            List<DayMedListClass> list = new List<DayMedListClass>();
            foreach (Medication medication in App.MedList)
            {
                foreach (Day day in medication.Days)
                {
                    if (day.Date == DateTime.Now.Date)
                    {
                        foreach (TimeSpan timeSpan in day.Time)
                        {
                            DayMedListClass temp = new DayMedListClass();
                            temp.Date = day.Date;
                            temp.Medication = medication;
                            temp.Name = medication.Name;
                            temp.Time = timeSpan;
                            list.Add(temp);
                        }
                    }
                }
            }
            Listyboxy.ItemsSource = list;
        }

        private void TakenBtn_OnClick(object sender, RoutedEventArgs e)
        {
            Button tempBtn = (Button) sender;
            var temp = tempBtn.DataContext;
            DayMedListClass temp2 = (DayMedListClass)temp;
            for (int x = 0; x < App.MedList.Count; x++)
            {
                if (temp2.Medication == App.MedList.ElementAt(x))
                {
                    for (int j = 0; j < App.MedList.ElementAt(x).Days.Count; j++)
                    {
                        if (temp2.Date == App.MedList.ElementAt(x).Days.ElementAt(j).Date)
                        {
                            for (int i = 0; i < App.MedList.ElementAt(x).Days.ElementAt(j).Time.Count; i++)
                            {
                                if (temp2.Time == App.MedList.ElementAt(x).Days.ElementAt(j).Time.ElementAt(i))
                                {
                                    if (tempBtn.Content.ToString() == "Taken")
                                    {
                                        App.MedList[x].Days[j].Taken[i] = true; // moet nog naar db
                                    }
                                    else
                                    {
                                        App.MedList[x].Days[j].Taken[i] = false; // moet nog naar db
                                    }
                                    
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}