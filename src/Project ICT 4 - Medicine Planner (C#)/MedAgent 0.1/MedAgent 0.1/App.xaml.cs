﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace MedAgent_0_1
{
    public partial class App : Application
    {
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }


        ///Global variables
        /// 
        /// 
        /// 
        public static Doctor PublicDoctor = new Doctor();
        public static Patient PublicPatient = new Patient();

        public static List<Medication> MedList = new List<Medication>();

        public static List<Patient> PatList = new List<Patient>();

        public static bool IsPatient { get; set; }

        public static int MedID = 0;

        public static int PatID = 0;


        public static int PrescriptionsCounter = 0;



        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            // Note that exceptions thrown by ApplicationBarItem.Click will not get caught here.
            UnhandledException += Application_UnhandledException;

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are being GPU accelerated with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;
            }

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();



        }

        public static void UpdateReminders()
        {
            int remNum = 0;
            string RemName = "Rem";
            while (ScheduledActionService.Find(RemName + remNum) != null)
            {
                ScheduledActionService.Remove(RemName + remNum);
                remNum++;
            }
            remNum = 0;
            foreach (Medication medication in MedList)
            {
                foreach (Day day in medication.Days)
                {
                    if (day.Date >= DateTime.Now.Date && day.Date < DateTime.Now.AddDays(5))
                    {
                        foreach (TimeSpan time in day.Time)
                        {
                            if (time != new TimeSpan(0, 0, 0, 0))
                            {
                                DateTime dt = day.Date;
                                TimeSpan ts = time;
                                dt = dt + ts;
                                if (dt >= DateTime.Now)
                                {
                                    Reminder r = new Reminder("Rem" + remNum)
                                    {
                                        Content = medication.Description+"\nClick for more info",
                                        BeginTime = dt,
                                        Title = medication.Name,
                                        NavigationUri = new Uri("/MedConfirmationPage.xaml", UriKind.Relative),
                                        RecurrenceType = RecurrenceInterval.None
                                    };
                                    ScheduledActionService.Add(r);
                                    remNum++;
                                }
                            }
                        }
                    }
                }
            }
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        public static bool InternetOn()
        {
            if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType !=
 Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None)
            {
                return true;
            }
            return false;
        }

        public static void ConnectErrorMsg()
        {
            MessageBoxResult m = MessageBox.Show("Verify your internet connection!", "Cannot connect!", MessageBoxButton.OK);
            if (m == MessageBoxResult.OK)
            {
                if (!InternetOn())
                {
                    ConnectionSettingsTask connSettingsTask = new ConnectionSettingsTask();
                    connSettingsTask.ConnectionSettingsType = ConnectionSettingsType.WiFi;
                    connSettingsTask.Show();
                }
            }
            //MessageBox.Show("Cannot connect!\n\rVerify your internet connection!");
        }
        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}