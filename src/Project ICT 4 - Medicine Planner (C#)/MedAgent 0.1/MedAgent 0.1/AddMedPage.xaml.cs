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
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Tasks;
using Microsoft.Phone;
using System.Windows.Navigation;


namespace MedAgent_0_1
{
    public partial class PanoramaPage1 : PhoneApplicationPage
    {
        MedCareCloudServiceReference.MedPlanServiceClient client;
        public PanoramaPage1()
        {
            InitializeComponent();
            camTask = new CameraCaptureTask();
            camTask.Completed += camTaskCompleted;

            /*Touch.FrameReported += (s, e) =>
            {
                if (e.GetPrimaryTouchPoint(CourseSlider).Action == TouchAction.Up)
                {
                    Item1.IsHitTestVisible = true;
                    Item2.IsHitTestVisible = true;
                    Item3.IsHitTestVisible = true;
                    Item4.IsHitTestVisible = true;
                }
            };*/



        }



        /// <summary>
        /// WARNING MONSTERVEEL CODE INCOMING
        /// </summary>



        //Panorama Item 1 

        private CameraCaptureTask camTask;

        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            camTask.Show();
        }

        private void camTaskCompleted(object sender, PhotoResult pr)
        {
            byte[] imgLocal;
            if (pr.ChosenPhoto != null)
            {
                imgLocal = new byte[(int)pr.ChosenPhoto.Length];
                pr.ChosenPhoto.Read(imgLocal, 0, imgLocal.Length);
                pr.ChosenPhoto.Seek(0, System.IO.SeekOrigin.Begin);
                var bitmapImage = PictureDecoder.DecodeJpeg(pr.ChosenPhoto);
                CameraImage.Source = bitmapImage;

                //Put the image in the list data
                App.MedList[App.MedID].MedPhoto = CameraImage.Source;
            }
        }



        //Panorama Item 2

        private void course_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            switch ((int)(CourseSlider.Value))
            {
                case 0: repeatInfo_txt.Text = "Daily";
                    App.MedList[App.MedID].Course = "Daily";
                    break;
                case 1: repeatInfo_txt.Text = "Every 2 days";
                    App.MedList[App.MedID].Course = "Every 2 days";
                    break;
                case 2: repeatInfo_txt.Text = "Every 3 days";
                    App.MedList[App.MedID].Course = "Every 3 days";
                    break;
                case 3: repeatInfo_txt.Text = "Every 4 days";
                    App.MedList[App.MedID].Course = "Every 4 days";
                    break;
                case 4: repeatInfo_txt.Text = "Every 5 days";
                    App.MedList[App.MedID].Course = "Every 5 days";
                    break;
                case 5: repeatInfo_txt.Text = "Weekly";
                    App.MedList[App.MedID].Course = "Weekly";
                    break;
                default: repeatInfo_txt.Text = "error";
                    App.MedList[App.MedID].Course = "N/A";
                    break;
            }
        }

        private void CourseSlider_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            Item4.IsHitTestVisible = false;
            Item3.IsHitTestVisible = false;
            Item2.IsHitTestVisible = false;
            Item1.IsHitTestVisible = false;
        }

        private void CourseSlider_MouseLeave(object sender, MouseEventArgs e)
        {
            Item1.IsHitTestVisible = true;
            Item2.IsHitTestVisible = true;
            Item3.IsHitTestVisible = true;
            Item4.IsHitTestVisible = true;
        }
        //Panorama Item 3

        public int Tablets1 = 0;
        public int Tablets2 = 0;
        public int Tablets3 = 0;
        public int Tablets4 = 0;
        public int Tablets5 = 0;
        public int Tablets6 = 0;



        private void ToggleButton2_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            TimeText2.Opacity = 1;
            TabletsText2.Opacity = 1;

            Time2.Opacity = 1;
            Time2.IsEnabled = true;
            AddTabletsButton2.Opacity = 1;
            AddTabletsButton2.IsEnabled = true;

        }

        private void ToggleButton2_Unchecked(object sender, RoutedEventArgs e)
        {
            TimeText2.Opacity = 0.4;
            TabletsText2.Opacity = 0.4;

            Time2.Opacity = 1;
            Time2.IsEnabled = false;
            AddTabletsButton2.Opacity = 1;
            AddTabletsButton2.IsEnabled = false;

            AddTabletsButton2.Content = 0;
            Tablets2 = 0;

        }

        private void ToggleButton3_Checked(object sender, RoutedEventArgs e)
        {
            TimeText3.Opacity = 1;
            TabletsText3.Opacity = 1;

            Time3.Opacity = 1;
            Time3.IsEnabled = true;
            AddTabletsButton3.Opacity = 1;
            AddTabletsButton3.IsEnabled = true;


        }

        private void ToggleButton3_Unchecked(object sender, RoutedEventArgs e)
        {
            TimeText3.Opacity = 0.4;
            TabletsText3.Opacity = 0.4;

            Time3.Opacity = 1;
            Time3.IsEnabled = false;
            AddTabletsButton3.Opacity = 1;
            AddTabletsButton3.IsEnabled = false;

            AddTabletsButton3.Content = 0;
            Tablets3 = 0;
        }

        private void ToggleButton4_Checked(object sender, RoutedEventArgs e)
        {
            TimeText4.Opacity = 1;
            TabletsText4.Opacity = 1;

            Time4.Opacity = 1;
            Time4.IsEnabled = true;
            AddTabletsButton4.Opacity = 1;
            AddTabletsButton4.IsEnabled = true;
        }

        private void ToggleButton4_Unchecked(object sender, RoutedEventArgs e)
        {
            TimeText4.Opacity = 0.4;
            TabletsText4.Opacity = 0.4;

            Time4.Opacity = 1;
            Time4.IsEnabled = false;
            AddTabletsButton4.Opacity = 1;
            AddTabletsButton4.IsEnabled = false;

            AddTabletsButton4.Content = 0;
            Tablets4 = 0;
        }

        private void ToggleButton5_Checked(object sender, RoutedEventArgs e)
        {
            TimeText5.Opacity = 1;
            TabletsText5.Opacity = 1;

            Time5.Opacity = 1;
            Time5.IsEnabled = true;
            AddTabletsButton5.Opacity = 1;
            AddTabletsButton5.IsEnabled = true;
        }

        private void ToggleButton5_Unchecked(object sender, RoutedEventArgs e)
        {
            TimeText5.Opacity = 0.4;
            TabletsText5.Opacity = 0.4;

            Time5.Opacity = 1;
            Time5.IsEnabled = false;
            AddTabletsButton5.Opacity = 1;
            AddTabletsButton5.IsEnabled = false;

            AddTabletsButton5.Content = 0;
            Tablets5 = 0;
        }

        private void ToggleButton6_Checked(object sender, RoutedEventArgs e)
        {
            TimeText6.Opacity = 1;
            TabletsText6.Opacity = 1;

            Time6.Opacity = 1;
            Time6.IsEnabled = true;
            AddTabletsButton6.Opacity = 1;
            AddTabletsButton6.IsEnabled = true;

            AddTabletsButton6.Content = 0;
            Tablets6 = 0;
        }

        private void ToggleButton6_Unchecked(object sender, RoutedEventArgs e)
        {
            TimeText6.Opacity = 0.4;
            TabletsText6.Opacity = 0.4;

            Time6.Opacity = 1;
            Time6.IsEnabled = false;
            AddTabletsButton6.Opacity = 1;
            AddTabletsButton6.IsEnabled = false;

            AddTabletsButton6.Content = 0;
            Tablets6 = 0;
        }


        private void TabletsSlider1_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            switch ((int)(TabletsSlider1.Value))
            {
                case 0: AddTabletsButton1.Content = "0";
                    Tablets1 = 0;
                    break;
                case 1: AddTabletsButton1.Content = "1";
                    Tablets1 = 1;
                    break;
                case 2: AddTabletsButton1.Content = "2";
                    Tablets1 = 2;
                    break;
                case 3: AddTabletsButton1.Content = "3";
                    Tablets1 = 3;
                    break;
                case 4: AddTabletsButton1.Content = "4";
                    Tablets1 = 4;
                    break;
                case 5: AddTabletsButton1.Content = "5";
                    Tablets1 = 5;
                    break;
                case 6: AddTabletsButton1.Content = "6";
                    Tablets1 = 6;
                    break;
                case 7: AddTabletsButton1.Content = "7";
                    Tablets1 = 7;
                    break;
                case 8: AddTabletsButton1.Content = "8";
                    Tablets1 = 8;
                    break;
                case 9: AddTabletsButton1.Content = "9";
                    Tablets1 = 9;
                    break;
                case 10: AddTabletsButton1.Content = "10";
                    Tablets1 = 10;
                    break;
                default: AddTabletsButton1.Content = "WTF??";
                    break;
            }

            TabletsTextBlock1.Text = Convert.ToString(Tablets1);
        }

        private void TabletsSlider2_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            switch ((int)(TabletsSlider2.Value))
            {
                case 0: AddTabletsButton2.Content = "0";
                    Tablets2 = 0;
                    break;
                case 1: AddTabletsButton2.Content = "1";
                    Tablets2 = 1;
                    break;
                case 2: AddTabletsButton2.Content = "2";
                    Tablets2 = 2;
                    break;
                case 3: AddTabletsButton2.Content = "3";
                    Tablets2 = 3;
                    break;
                case 4: AddTabletsButton2.Content = "4";
                    Tablets2 = 4;
                    break;
                case 5: AddTabletsButton2.Content = "5";
                    Tablets2 = 5;
                    break;
                case 6: AddTabletsButton2.Content = "6";
                    Tablets2 = 6;
                    break;
                case 7: AddTabletsButton2.Content = "7";
                    Tablets2 = 7;
                    break;
                case 8: AddTabletsButton2.Content = "8";
                    Tablets2 = 8;
                    break;
                case 9: AddTabletsButton2.Content = "9";
                    Tablets2 = 9;
                    break;
                case 10: AddTabletsButton2.Content = "10";
                    Tablets2 = 10;
                    break;
                default: AddTabletsButton2.Content = "WTF??";
                    break;
            }
        }

        private void TabletsSlider3_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            switch ((int)(TabletsSlider3.Value))
            {
                case 0: AddTabletsButton3.Content = "0";
                    Tablets3 = 0;
                    break;
                case 1: AddTabletsButton3.Content = "1";
                    Tablets3 = 1;
                    break;
                case 2: AddTabletsButton3.Content = "2";
                    Tablets3 = 2;
                    break;
                case 3: AddTabletsButton3.Content = "3";
                    Tablets3 = 3;
                    break;
                case 4: AddTabletsButton3.Content = "4";
                    Tablets3 = 4;
                    break;
                case 5: AddTabletsButton3.Content = "5";
                    Tablets3 = 5;
                    break;
                case 6: AddTabletsButton3.Content = "6";
                    Tablets3 = 6;
                    break;
                case 7: AddTabletsButton3.Content = "7";
                    Tablets3 = 7;
                    break;
                case 8: AddTabletsButton3.Content = "8";
                    Tablets3 = 8;
                    break;
                case 9: AddTabletsButton3.Content = "9";
                    Tablets3 = 9;
                    break;
                case 10: AddTabletsButton3.Content = "10";
                    Tablets3 = 10;
                    break;
                default: AddTabletsButton3.Content = "WTF??";
                    break;
            }
        }

        private void TabletsSlider4_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            switch ((int)(TabletsSlider4.Value))
            {
                case 0: AddTabletsButton4.Content = "0";
                    Tablets4 = 0;
                    break;
                case 1: AddTabletsButton4.Content = "1";
                    Tablets4 = 1;
                    break;
                case 2: AddTabletsButton4.Content = "2";
                    Tablets4 = 2;
                    break;
                case 3: AddTabletsButton4.Content = "3";
                    Tablets4 = 3;
                    break;
                case 4: AddTabletsButton4.Content = "4";
                    Tablets4 = 4;
                    break;
                case 5: AddTabletsButton4.Content = "5";
                    Tablets4 = 5;
                    break;
                case 6: AddTabletsButton4.Content = "6";
                    Tablets4 = 6;
                    break;
                case 7: AddTabletsButton4.Content = "7";
                    Tablets4 = 7;
                    break;
                case 8: AddTabletsButton4.Content = "8";
                    Tablets4 = 8;
                    break;
                case 9: AddTabletsButton4.Content = "9";
                    Tablets4 = 9;
                    break;
                case 10: AddTabletsButton4.Content = "10";
                    Tablets4 = 10;
                    break;
                default: AddTabletsButton4.Content = "WTF??";
                    break;
            }
        }

        private void TabletsSlider5_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            switch ((int)(TabletsSlider5.Value))
            {
                case 0: AddTabletsButton5.Content = "0";
                    Tablets5 = 0;
                    break;
                case 1: AddTabletsButton5.Content = "1";
                    Tablets5 = 1;
                    break;
                case 2: AddTabletsButton5.Content = "2";
                    Tablets5 = 2;
                    break;
                case 3: AddTabletsButton5.Content = "3";
                    Tablets5 = 3;
                    break;
                case 4: AddTabletsButton5.Content = "4";
                    Tablets5 = 4;
                    break;
                case 5: AddTabletsButton5.Content = "5";
                    Tablets5 = 5;
                    break;
                case 6: AddTabletsButton5.Content = "6";
                    Tablets5 = 6;
                    break;
                case 7: AddTabletsButton5.Content = "7";
                    Tablets5 = 7;
                    break;
                case 8: AddTabletsButton5.Content = "8";
                    Tablets5 = 8;
                    break;
                case 9: AddTabletsButton5.Content = "9";
                    Tablets5 = 9;
                    break;
                case 10: AddTabletsButton5.Content = "10";
                    Tablets5 = 10;
                    break;
                default: AddTabletsButton5.Content = "WTF??";
                    break;
            }
        }

        private void TabletsSlider6_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            switch ((int)(TabletsSlider6.Value))
            {
                case 0: AddTabletsButton6.Content = "0";
                    Tablets6 = 0;
                    break;
                case 1: AddTabletsButton6.Content = "1";
                    Tablets6 = 1;
                    break;
                case 2: AddTabletsButton6.Content = "2";
                    Tablets6 = 2;
                    break;
                case 3: AddTabletsButton6.Content = "3";
                    Tablets6 = 3;
                    break;
                case 4: AddTabletsButton6.Content = "4";
                    Tablets6 = 3;
                    break;
                case 5: AddTabletsButton6.Content = "5";
                    Tablets6 = 4;
                    break;
                case 6: AddTabletsButton6.Content = "6";
                    Tablets6 = 5;
                    break;
                case 7: AddTabletsButton6.Content = "7";
                    Tablets6 = 7;
                    break;
                case 8: AddTabletsButton6.Content = "8";
                    Tablets6 = 8;
                    break;
                case 9: AddTabletsButton6.Content = "9";
                    Tablets6 = 9;
                    break;
                case 10: AddTabletsButton6.Content = "10";
                    Tablets6 = 10;
                    break;
                default: AddTabletsButton6.Content = "WTF??";
                    break;
            }
        }


        private void AddTabletsButton1_OnClick(object sender, RoutedEventArgs e)
        {

            //Al de rest disablen

            Item1.IsEnabled = false;

            Item2.IsEnabled = false;

            Item3.IsEnabled = false;

            SliderPanel1.Visibility = Visibility.Visible;




        }

        private void ConfirmTabletsButton1_OnClick(object sender, RoutedEventArgs e)
        {

            //Al de rest disablen

            Item1.IsEnabled = true;

            Item2.IsEnabled = true;

            Item3.IsEnabled = true;

            SliderPanel1.Visibility = Visibility.Collapsed;

        }

        private void AddTabletsButton2_OnClick(object sender, RoutedEventArgs e)
        {
            SliderPanel2.Visibility = Visibility.Visible;
        }

        private void ConfirmTabletsButton2_OnClick(object sender, RoutedEventArgs e)
        {
            SliderPanel2.Visibility = Visibility.Collapsed;
        }

        private void AddTabletsButton3_OnClick(object sender, RoutedEventArgs e)
        {
            SliderPanel3.Visibility = Visibility.Visible;
        }

        private void ConfirmTabletsButton3_OnClick(object sender, RoutedEventArgs e)
        {
            SliderPanel3.Visibility = Visibility.Collapsed;
        }

        private void AddTabletsButton4_OnClick(object sender, RoutedEventArgs e)
        {
            SliderPanel4.Visibility = Visibility.Visible;
        }

        private void ConfirmTabletsButton4_OnClick(object sender, RoutedEventArgs e)
        {
            SliderPanel4.Visibility = Visibility.Collapsed;
        }

        private void AddTabletsButton5_OnClick(object sender, RoutedEventArgs e)
        {
            SliderPanel5.Visibility = Visibility.Visible;
        }

        private void ConfirmTabletsButton5_OnClick(object sender, RoutedEventArgs e)
        {
            SliderPanel5.Visibility = Visibility.Collapsed;
        }

        private void AddTabletsButton6_OnClick(object sender, RoutedEventArgs e)
        {
            SliderPanel6.Visibility = Visibility.Visible;
        }

        private void ConfirmTabletsButton6_OnClick(object sender, RoutedEventArgs e)
        {
            SliderPanel6.Visibility = Visibility.Collapsed;
        }




        //private Medication med1 = new Medication();



        ////////////////////////////////////////////////<-----------EVENTS------------>///////////////////////////////////////////////////////
        /// 
        /// 
        /// 
        //Put all values in a global variabel when their values are changed



        //Panorama Item 1

        private void NameTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            App.MedList[App.MedID].Name = NameTxtBox.Text;
            

        }



        private void DescrTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            App.MedList[App.MedID].Description = DescrTxtBox.Text;

        }




        //Panorama Item 2

        private void StartDate_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {

            App.MedList[App.MedID].StartDate = (DateTime)StartDate.Value;


        }

        private void EndDate_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {

            App.MedList[App.MedID].EndDate = (DateTime)EndDate.Value;

        }

        //Panorama Item 3

        private void Time1_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {

            App.MedList[App.MedID].Times[0] = (DateTime)Time2.Value;
            // WAT IS DIT??? (dries)
        }

        private void Time2_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {

            App.MedList[App.MedID].Times[1] = (DateTime)Time2.Value;
            //WAT IS DIT??? (dries)
        }

        private void Time3_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {

            App.MedList[App.MedID].Times[1] = (DateTime)Time2.Value;
            //WAT IS DIT??? (dries)
        }

        private void Time4_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {

            App.MedList[App.MedID].Times[1] = (DateTime)Time2.Value;
            //WAT IS DIT??? (dries)
        }

        private void Time5_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {

            App.MedList[App.MedID].Times[1] = (DateTime)Time2.Value;
            //WAT IS DIT??? (dries)
        }

        private void Time6_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {

            App.MedList[App.MedID].Times[1] = (DateTime)Time2.Value;
            //WAT IS DIT??? (dries)
        }



        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            

            //First check if all fields are filled in except the photo and the time reminders.
            if (App.MedList[App.MedID].Name != null && App.MedList[App.MedID].StartDate != null &&
                App.MedList[App.MedID].EndDate != null && App.MedList[App.MedID].Description != null
                && App.MedList[App.MedID].Course != null)
            {
                //The current value of the startdate is always the current date so when we don't change it we have to assign its value still
                App.MedList[App.MedID].StartDate = (DateTime) StartDate.Value;

                //Write the variables medication in the medlist with the current ID to the database.
                client = new MedAgent_0_1.MedCareCloudServiceReference.MedPlanServiceClient();
                client.CreatePrescriptionAsync(App.MedList[App.MedID].Name, App.MedList[App.MedID].StartDate, App.MedList[App.MedID].EndDate, App.MedList[App.MedID].Amount,
                    App.MedList[App.MedID].Times[0].TimeOfDay,
                    App.MedList[App.MedID].Times[1].TimeOfDay,
                    App.MedList[App.MedID].Times[2].TimeOfDay,
                    App.MedList[App.MedID].Times[3].TimeOfDay,
                    App.MedList[App.MedID].Times[4].TimeOfDay,
                    App.MedList[App.MedID].Times[5].TimeOfDay,
                    App.MedList[App.MedID].Description,
                    App.MedList[App.MedID].Course,
                    App.PublicPatient.Id,
                    "pillen",
                    'n','n','n','n','n','n');


                client.CreatePrescriptionCompleted += new EventHandler<MedCareCloudServiceReference.CreatePrescriptionCompletedEventArgs>(client_CreatePrescriptionCompleted);



                App.MedID++;

                NavigationService.Navigate(new Uri(string.Format("/PatientFile.xaml"), UriKind.Relative));

            }

            else
            {
                MessageBox.Show("Not all required fields have been filled in yet.");
            }



            //Werkt van geen kante ='((
            /*
            for (int i = 0; i < 6; i++)
            {
                if (ScheduledActionService.Find("rem" + (i + 1)) != null)
                {
                    ScheduledActionService.Remove("rem" + (i + 1)); 
                }
            }
            

            if ((bool)ToggleButton1.IsChecked)
            {
                App.MedList[App.MedID].reminder1 = new Reminder("rem1")
                {
                    BeginTime = (DateTime)Time1.Value,
                    Content = "Take pills",  // welke med toevoegen hier
                    ExpirationTime = ((DateTime)Time1.Value).AddDays(1),
                    NavigationUri = new Uri("/TestPage1.xaml", UriKind.Relative),
                    RecurrenceType = RecurrenceInterval.Daily,
                    Title = "Take med title" // change this
                };
                ScheduledActionService.Add(App.MedList[App.MedID].reminder1);
            }
            if ((bool)ToggleButton2.IsChecked)
            {
                App.MedList[App.MedID].reminder2 = new Reminder("rem2")
                {
                    BeginTime = (DateTime)Time2.Value,
                    Content = "Take pills",  // welke med toevoegen hier
                    ExpirationTime = ((DateTime)Time2.Value).AddDays(1),
                    NavigationUri = new Uri("/TestPage1.xaml", UriKind.Relative),
                    RecurrenceType = RecurrenceInterval.Daily,
                    Title = "Take med title" // change this
                };
                ScheduledActionService.Add(App.MedList[App.MedID].reminder2);
            }
            if ((bool)ToggleButton3.IsChecked)
            {
                App.MedList[App.MedID].reminder3 = new Reminder("rem3")
                    {
                        BeginTime = (DateTime)Time3.Value,
                        Content = "Take pills", // welke med toevoegen hier
                        ExpirationTime = ((DateTime)Time3.Value).AddDays(1),
                        NavigationUri = new Uri("/TestPage1.xaml", UriKind.Relative),
                        RecurrenceType = RecurrenceInterval.Daily,
                        Title = "Take med title" // change this
                    };
                ScheduledActionService.Add(App.MedList[App.MedID].reminder3);
            }

            if ((bool)ToggleButton4.IsChecked)
            {
                App.MedList[App.MedID].reminder4 = new Reminder("rem4")
                {
                    BeginTime = (DateTime)Time4.Value,
                    Content = "Take pills", // welke med toevoegen hier
                    ExpirationTime = ((DateTime)Time4.Value).AddDays(1),
                    NavigationUri = new Uri("/TestPage1.xaml", UriKind.Relative),
                    RecurrenceType = RecurrenceInterval.Daily,
                    Title = "Take med title" // change this
                };
                ScheduledActionService.Add(App.MedList[App.MedID].reminder4);
            }
            if ((bool)ToggleButton5.IsChecked)
            {
                App.MedList[App.MedID].reminder5 = new Reminder("rem5")
                {
                    BeginTime = (DateTime)Time5.Value,
                    Content = "Take pills", // welke med toevoegen hier
                    ExpirationTime = ((DateTime)Time5.Value).AddDays(1),
                    NavigationUri = new Uri("/TestPage1.xaml", UriKind.Relative),
                    RecurrenceType = RecurrenceInterval.Daily,
                    Title = "Take med title" // change this
                };
                ScheduledActionService.Add(App.MedList[App.MedID].reminder5);
            }
            if ((bool)ToggleButton6.IsChecked)
            {
                App.MedList[App.MedID].reminder6 = new Reminder("rem6")
                {
                    BeginTime = (DateTime)Time6.Value,
                    Content = "Take pills", // welke med toevoegen hier
                    ExpirationTime = ((DateTime)Time6.Value).AddDays(1),
                    NavigationUri = new Uri("/TestPage1.xaml", UriKind.Relative),
                    RecurrenceType = RecurrenceInterval.Daily,
                    Title = "Take med title" // change this
                };
                ScheduledActionService.Add(App.MedList[App.MedID].reminder6);
            }

            */

        }

        void client_CreatePrescriptionCompleted(object sender, MedCareCloudServiceReference.CreatePrescriptionCompletedEventArgs e)
        {
            if (e.Result == "success")
                MessageBox.Show("Prescription added! :)");
            else
                MessageBox.Show("Oops, something went wrong :(\n\rFailed to add prescription\n\rError: " + e.Result);
        }



    }
}