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

namespace TailSpin.Phone.Adapters
{
    /// <summary>
    /// The Tailspin Surveys application uses the Isolated Storage file system to store both survey questions
    /// and survey answers. One file is used to store questions and another for answers.
    /// If your scenario requires simultaneous updating questions or answers from different 
    /// places in the application, then using a database would appropriate.
    /// </summary>
    public class IsolatedStorageFacade : IIsolatedStorageFacade
    {
        private Stream outputStream;
        private BinaryWriter outputWriter;
        private IsolatedStorageFile fileSystem;

        public BinaryWriter OpenBinaryWriter(string outputFileName)
        {
            this.fileSystem = IsolatedStorageFile.GetUserStoreForApplication();

            if (fileSystem.FileExists(outputFileName))
            {
                fileSystem.DeleteFile(outputFileName);
            }

            this.outputStream = fileSystem.OpenFile(outputFileName, FileMode.Create);
            this.outputWriter = new BinaryWriter(outputStream);
            return this.outputWriter;
        }

        public void Dispose()
        {
            if (outputStream != null)
            {
                this.Close();
            }
        }

        private void Close()
        {
            this.outputStream.Close();
            this.outputStream.Dispose();
            this.outputWriter.Dispose();
            this.fileSystem.Dispose();
            this.outputStream = null;
            this.outputWriter = null;
            this.fileSystem = null;
        }
    }
}
