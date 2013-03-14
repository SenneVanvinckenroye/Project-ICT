using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Globalization;

namespace CalendarControl
{
    public partial class LCalendar : UserControl
    {
        #region Proprietà Private
        private Dictionary<ShortDateString, List<CalendarEvent>> mEvents;
        private DateTime mCurrentDate;
        private event EventHandler<DayClickedEventArgs> dayClicked;
        #endregion

        #region Costruttore

        public LCalendar()
        {
            InitializeComponent();
            InitializeObjects();
            PopulateDaysTextBlock();
        }

        #endregion

        #region Metodi Pubblici
        /// <summary>
        /// Aggiorna il layout del calendario
        /// </summary>
        public void Refresh()
        {
            InizializeCalendar(mCurrentDate);
        }
        /// <summary>
        /// Evento che viene scatenato quando si preme su un giorno
        /// </summary>
        public event EventHandler<DayClickedEventArgs> OnDayClicked
        {
            add
            {
                dayClicked += new EventHandler<DayClickedEventArgs>(value);
            }
            remove
            {
                dayClicked -= new EventHandler<DayClickedEventArgs>(value);
            }
        }
        /// <summary>
        /// Aggiunge un nuovo evento alla lista degli eventi
        /// </summary>
        public void AddNewEvent(DateTime date, CalendarEvent _event)
        {
            ShortDateString sDate = new ShortDateString(date);

            if (!mEvents.ContainsKey(sDate))
                mEvents.Add(sDate, new List<CalendarEvent>() { _event });
            else
            {
                List<CalendarEvent> list = mEvents[sDate];

                if (list == null)
                    list = new List<CalendarEvent>();

                list.Add(_event);
            }

        }
        /// <summary>
        /// Rimuove un evento in particolare di una data conosciuta
        /// </summary>
        public bool RemoveEvent(DateTime date, CalendarEvent _event)
        {
            ShortDateString sDate = new ShortDateString(date);
            if (!mEvents.ContainsKey(sDate))
                return false;
            else
            {
                List<CalendarEvent> list = mEvents[sDate];

                if (list == null)
                    return false;
                else return list.Remove(_event);
            }
        }
        /// <summary>
        /// Rimuove un evento in particolare di una data conosciuta
        /// </summary>
        public bool RemoveEvent(DateTime date, Func<CalendarEvent, bool> predicate)
        {

            ShortDateString sDate = new ShortDateString(date);
            if (!mEvents.ContainsKey(sDate))
                return false;
            else
            {
                List<CalendarEvent> list = mEvents[sDate];

                if (list == null)
                    return false;
                else
                {
                    CalendarEvent ev = list.First(predicate);
                    if (ev != null)
                        return list.Remove(ev);
                    else return false;
                }
            }
        }
        /// <summary>
        /// Rimuove gli eventi per una data
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool RemoveDateEvents(DateTime date)
        {
            ShortDateString sDate = new ShortDateString(date);
            return mEvents.Remove(sDate);

        }
        /// <summary>
        /// Cancella tutti gli eventi memorizzati nel calendario
        /// </summary>
        public void ClearEvents()
        {
            mEvents.Clear();
        }
        /// <summary>
        /// Restituisce la lista degli eventi per quella data
        /// </summary>
        private List<CalendarEvent> FindEventsByDate(DateTime dateTime)
        {
            ShortDateString sDate = new ShortDateString(dateTime);

            if (mEvents.ContainsKey(sDate))
                return mEvents[sDate];
            else return null;
        }
        #endregion

        #region Eventi
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {

            InizializeCalendar(DateTime.Now);
        }

        private void ba_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button && dayClicked != null)
            {
                DateTime date = new DateTime(mCurrentDate.Year, mCurrentDate.Month, (int)(sender as Button).Tag);
                dayClicked(sender, new DayClickedEventArgs(date, FindEventsByDate(date)));
            }
        }

        #endregion

        #region Metodi Privati

        private void InitializeObjects()
        {
            mEvents = new Dictionary<ShortDateString, List<CalendarEvent>>();
            mDefaultEventsForegroundBrush = new SolidColorBrush(Colors.White);
            mDefaultEventsBackgroundBrush = (SolidColorBrush)Resources["PhoneAccentBrush"];
        }

        private DateTime BringToFirst(DateTime datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, 1);
        }


        private void PopulateDaysTextBlock()
        {
            tLUN.Text = DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Monday).ToString().Substring(0, 3).ToUpper();
            tMAR.Text = DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Tuesday).ToString().Substring(0, 3).ToUpper();
            tMER.Text = DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Wednesday).ToString().Substring(0, 3).ToUpper();
            tGIO.Text = DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Thursday).ToString().Substring(0, 3).ToUpper();
            tVEN.Text = DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Friday).ToString().Substring(0, 3).ToUpper();
            tSAB.Text = DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Saturday).ToString().Substring(0, 3).ToUpper();
            tDOM.Text = DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Sunday).ToString().Substring(0, 3).ToUpper();
        }
        private void InizializeCalendar(DateTime dateTime)
        {
            SetMonthYear(dateTime);

            int days = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            DateTime firstOfMonth = BringToFirst(dateTime);
            DayOfWeek startingDay = firstOfMonth.DayOfWeek;


            ClearDays();

            int riga = 2;
            int colonna = GetDayNumber(startingDay);
            for (int i = 0; i < days; i++)
            {
                Border bord = new Border();
                bord.Name = "brd" + (i + 1).ToString();

                bord.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                bord.Margin = new Thickness(0);

                Button ba = new Button();
                ba.BorderBrush = new SolidColorBrush(Colors.Transparent);
                ba.Width = 99;
                ba.Height = 99;
                ba.Tag = (i + 1);
                ba.Margin = new Thickness(0);
                ba.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                ba.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                ba.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                ba.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                ba.Content = (i + 1).ToString();
                ba.Click += new RoutedEventHandler(ba_Click);

                if (dateTime.Year == DateTime.Now.Year && dateTime.Month == DateTime.Now.Month && ba.Content.ToString() == DateTime.Now.Day.ToString())
                {
                    ba.Background = new SolidColorBrush(Colors.Orange);
                }

                //controllo sull'evento esistente

                if (HasEvents(new ShortDateString(firstOfMonth.AddDays(i))))
                {
                    bord.Background = EventsBackgroundBrush;
                    ba.Foreground = EventsForegroundBrush;
                    //bord.CornerRadius = new CornerRadius(15);
                }

                bord.Child = ba;

                AddElementToGrid(riga, colonna++, bord);

                //countDay++;
                if (colonna % 7 == 0)
                {
                    riga++;
                    colonna = 0;
                }
            }

            mCurrentDate = dateTime;
            LayoutRoot.UpdateLayout();

        }
        private bool HasEvents(ShortDateString sDate)
        {
            if (mEvents.ContainsKey(sDate))
                return mEvents[sDate] != null && mEvents[sDate].Count > 0;
            else return false;
        }
        private void SetMonthYear(DateTime dateTime)
        {
            tbMeseAnno.Text = String.Format("{0} {1}", dateTime.ToString("MMMM").ToUpper(), dateTime.Year);
        }

        private int GetDayNumber(DayOfWeek d)
        {
            if (d == DayOfWeek.Sunday)
            {
                return 6;
            }
            else return ((int)d) - 1;
        }

        private void ClearDays()
        {
            LayoutRoot.Children.ToList().ForEach(x =>
            {
                if (x is FrameworkElement && (x as FrameworkElement).Name.StartsWith("brd"))
                {
                    LayoutRoot.Children.Remove(x);
                }
            });
        }

        private void AddElementToGrid(int riga, int colonna, FrameworkElement el)
        {
            Grid.SetRow(el, riga);
            Grid.SetColumn(el, colonna);
            LayoutRoot.Children.Add(el);
        }

        private void OnChangeMonth(object sender, RoutedEventArgs e)
        {
            Button butSender = sender as Button;

            if (butSender.Name == NextBtn.Name)
            {
                mCurrentDate = mCurrentDate.AddMonths(+1);
            }
            else if (butSender.Name == BackBtn.Name)
            {
                mCurrentDate = mCurrentDate.AddMonths(-1);
            }

            InizializeCalendar(mCurrentDate);
        }
        #endregion

        #region Nested Classes
        public class DayClickedEventArgs : EventArgs
        {
            public DateTime SelectedDate { get; set; }
            public List<CalendarEvent> Events { get; set; }
            public DayClickedEventArgs(DateTime dt, List<CalendarEvent> evn)
            {
                SelectedDate = dt;
                Events = evn;
            }
        }
        public class ShortDateString
        {
            String Value { get; set; }
            DateTime DateTimeValue { get; set; }
            public ShortDateString(DateTime dt)
            {
                Value = dt.ToShortDateString();
                DateTimeValue = dt;
            }
            public override int GetHashCode()
            {
                return Value.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                if (obj is ShortDateString)
                    return Value.Equals((obj as ShortDateString).Value);
                else return base.Equals(obj);
            }
        }
        public class CalendarEvent
        {
            public DateTime DateWhen { get; set; }
            public String Description { get; set; }
            public object Data { get; set; }
            public CalendarEvent(String _description, DateTime _when, object _data = null)
            {
                DateWhen = _when;
                Description = _description;
                Data = _data;
            }
        }
        #endregion

        #region Brushes and GUI
        private SolidColorBrush mDefaultHolidayBrush;
        private SolidColorBrush mDefaultEventsBackgroundBrush;
        private SolidColorBrush mDefaultEventsForegroundBrush;

        public SolidColorBrush HolidayBrush
        {
            get
            {
                return mDefaultHolidayBrush;
            }
            set
            {
                mDefaultHolidayBrush = value;
            }
        }
        public SolidColorBrush EventsBackgroundBrush
        {
            get
            {
                return mDefaultEventsBackgroundBrush;
            }
            set
            {
                mDefaultEventsBackgroundBrush = value;
            }
        }

        public SolidColorBrush EventsForegroundBrush
        {
            get
            {
                return mDefaultEventsForegroundBrush;
            }
            set
            {
                mDefaultEventsForegroundBrush = value;
            }
        }

        public ImageSource NextImageSource
        {
            get
            {
                return imgNextMonth.Source;
            }
            set
            {
                imgNextMonth.Source = value;
            }
        }

        public ImageSource PrevImageSource
        {
            get
            {
                return imgPrevMonth.Source;
            }
            set
            {
                imgPrevMonth.Source = value;
            }
        }
        public Brush ButtonBorderBrush
        {
            get
            {
                return BackBtn.BorderBrush;
            }
            set
            {
                BackBtn.BorderBrush = value;
                NextBtn.BorderBrush = value;

            }
        }
        public Brush ButtonBackground
        {
            get
            {
                return BackBtn.Background;
            }
            set
            {
                BackBtn.Background = value;
                NextBtn.Background = value;

            }
        }
        #endregion
    }
}
