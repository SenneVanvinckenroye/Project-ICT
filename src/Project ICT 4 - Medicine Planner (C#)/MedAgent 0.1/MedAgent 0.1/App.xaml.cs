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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
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
        public static Patient PublicPatient = new Patient(); 

        public static List<Medication> MedList = new List<Medication>();

        public static int MedID = 0;

        public DateTime Time1 { get; set; }
        public DateTime Time2 { get; set; }
        public DateTime Time3 { get; set; }
        public DateTime Time4 { get; set; }
        public DateTime Time5 { get; set; }
        public DateTime Time6 { get; set; }

        public string MedName { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        public int Amount1 { get; set; }
        public int Amount2 { get; set; }
        public int Amount3 { get; set; }
        public int Amount4 { get; set; }
        public int Amount5 { get; set; }
        public int Amount6 { get; set; }









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

            //test code!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            App.MedList.Add(new Medication()
            {
                Amount = new int[] { 1, 2, 3, 4, 5, 6 },
                Description = "testmed",
                EndDate = new DateTime(2013, 04, 30),
                Interval = 3,
                Name = "TestMed",
                StartDate = new DateTime(2013, 04, 1),

                Taken = new List<List<bool>>(){
                            new List<bool>(){true,true,true,false,false,false},
                            new List<bool>(){false,false,false,false,false,false},
                            new List<bool>(){true,false,false,false,false,false},
                            new List<bool>(){true,true,true,false,false,false},
                            new List<bool>(){true,true,true,false,false,false},
                            new List<bool>(){true,true,true,false,false,false},
                            new List<bool>(){true,true,true,false,false,false},
                            new List<bool>(){true,true,true,false,false,false},
                            new List<bool>(){true,true,true,false,false,false},
                            new List<bool>(){true,true,true,false,false,false},
                            new List<bool>(){true,true,true,false,false,false},
                            new List<bool>(){true,true,true,false,false,false},
                            new List<bool>(){true,true,true,false,false,false},
                            new List<bool>(){true,true,true,false,false,false},
                            new List<bool>(){true,true,true,false,false,false},
                            new List<bool>(){true,true,true,false,false,false}
                        },

                Time = new DateTime[]{
                            new DateTime(2013,04,24,06,00,00),
                            new DateTime(2013,04,24,08,00,00), 
                            new DateTime(), 
                            new DateTime(), 
                            new DateTime(), 
                            new DateTime()
                        },
            });
            //test code!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

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