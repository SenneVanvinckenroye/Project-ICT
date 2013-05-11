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
    public partial class DayMedPage : PhoneApplicationPage
    {

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string stringDate;
            if (this.NavigationContext.QueryString.TryGetValue("Date", out stringDate))
            {
                DateTime Date;
                DateTime.TryParse(stringDate, out Date);
                PopulateList(Date);
            }
        }

        public DayMedPage()
        {
            InitializeComponent();
        }

        private void PopulateList(DateTime date)
        {
            List<DayMedListClass> list = new List<DayMedListClass>();
            foreach (Medication medication in App.MedList)
            {
                foreach (Day day in medication.Days)
                {
                    if (day.Date == date)
                    {
                        foreach (TimeSpan timeSpan in day.Time)
                        {
                            DayMedListClass temp = new DayMedListClass();
                            temp.Date = date.Date;
                            temp.Medication = medication;
                            temp.Name = medication.Name;
                            temp.Time = timeSpan;
                            list.Add(temp);
                        }
                    }
                }
            }
            DayMedList.ItemsSource = list;
        }

        private void DayMedList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DayMedList != null && DayMedList.SelectedItem != null)
            {
                //Data hale uit geselecteerde item
                DayMedListClass tempdata = (DayMedListClass)DayMedList.SelectedItem;
                Medication meddata = (Medication)tempdata.Medication;

                // reset selection of ListBox 
                ((ListBox)sender).SelectedIndex = -1;

                //Data doorsture als ge naar de volgende pagina ga
                FrameworkElement root = Application.Current.RootVisual as FrameworkElement;

                root.DataContext = meddata;



                // change page navigation 
                NavigationService.Navigate(new Uri(string.Format("/OverviewPage.xaml"), UriKind.Relative));
            }
        }
    }
}