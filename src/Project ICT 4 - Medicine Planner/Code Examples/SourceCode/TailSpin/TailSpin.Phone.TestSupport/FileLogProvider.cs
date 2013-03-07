//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using Microsoft.Silverlight.Testing.Harness;

namespace TailSpin.Phone.TestSupport
{
    public class FileLogProvider : LogProvider
    {
        public const string TESTRESULTFILENAME = @"TestResults\testresults.txt";
        protected override void ProcessRemainder(LogMessage message)
        {
            base.ProcessRemainder(message);
            AppendToFile(message);
        }

        public override void Process(LogMessage logMessage)
        {
            AppendToFile(logMessage);
        }

        private void AppendToFile(LogMessage logMessage)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            var carriageReturnBytes = encoding.GetBytes(new[] { '\r', '\n' });

            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!store.DirectoryExists("TestResults"))
                {
                    store.CreateDirectory("TestResults");
                }
                using (IsolatedStorageFileStream isoStream =
                    store.OpenFile(TESTRESULTFILENAME, FileMode.Append))
                {
                    var byteArray = encoding.GetBytes(logMessage.Message);
                    isoStream.Write(byteArray, 0, byteArray.Length);
                    isoStream.Write(carriageReturnBytes, 0, carriageReturnBytes.Length);
                }
            }
        }
    }
}
