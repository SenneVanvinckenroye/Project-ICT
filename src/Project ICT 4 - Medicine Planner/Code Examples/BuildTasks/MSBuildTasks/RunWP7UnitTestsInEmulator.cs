//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.SmartDevice.Connectivity;

namespace MSBuildTasks
{
    public class RunWP7UnitTestsInEmulator : Task
    {
        private int defaultNumberOfMiliSecondsToProcess = 50000;

        [Required]
        public string PathToUnitTestXapFile { get; set; }

        [Required]
        public string ProductGuid { get; set; }

        public int NumberOfMilliSecondsToProcess { get; set; }

        public override bool Execute()
        {
            if (NumberOfMilliSecondsToProcess == 0)
            {
                Log.LogMessage(string.Format("Using default processing time: {0}", defaultNumberOfMiliSecondsToProcess));
                NumberOfMilliSecondsToProcess = defaultNumberOfMiliSecondsToProcess;
            }

            Log.LogMessage(string.Format("NumberOfMilliSecondsToProcess: {0}", NumberOfMilliSecondsToProcess));

            Log.LogMessage("Running tests in XAP: " + PathToUnitTestXapFile);
            var productGuid = new Guid(ProductGuid);

            var emulator = GetWP7Emulator();
            Log.LogMessage("Connecting to Emulator.");
            emulator.Connect();
            Log.LogMessage("After call to Connect().");

            //Deploy application
            if (emulator.IsApplicationInstalled(productGuid))
            {
                emulator.GetApplication(productGuid).Uninstall();
            }

            emulator.InstallApplication(productGuid, productGuid, "NormalApp", null, PathToUnitTestXapFile);

            //Run application
            var application = emulator.GetApplication(productGuid);
            application.Launch();

            //Need to replace with with a check of when the application finishes running, like while (application.IsRunning()){}
            Thread.Sleep(NumberOfMilliSecondsToProcess);

            //Get Results from Isolated Store on device
            var isostorefile = application.GetIsolatedStore();
            var localTestResultFileName = "EmulatorTestResults-" + productGuid + ".txt";
            isostorefile.ReceiveFile(@"\TestResults\testresults.txt", localTestResultFileName, true);

            var testResults = File.ReadAllText(localTestResultFileName);

            if (testResults.Contains("Exception:"))
            {
                Log.LogError("Failing test found. Look for exception in: " + localTestResultFileName);
                return false;
            }

            Log.LogMessage("All tests passed.");

            emulator.Disconnect();

            return true;
        }

        private Device GetWP7Emulator()
        {
            var manager = new DatastoreManager(CultureInfo.CurrentUICulture.LCID);
            var wp7Platform = manager.GetPlatforms().Single(platform => platform.Name == "Windows Phone 7");
            return wp7Platform.GetDevices().Single(device => device.Name == "Windows Phone Emulator");
        }
    }
}
