//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using Microsoft.Phone.Tasks;

namespace TailSpin.PhoneClient.Adapters
{
    public class CameraCaptureTaskAdapter : ICameraCaptureTask
    {
        public CameraCaptureTaskAdapter()
        {
            WrappedSubject = new CameraCaptureTask();
            AttachToEvents();
        }

        private CameraCaptureTask WrappedSubject { get; set; }

        public SettablePhotoResult TaskEventArgs
        {
            get
            {
                return new SettablePhotoResult(WrappedSubject.TaskEventArgs);
            }
            set
            {
                WrappedSubject.TaskEventArgs = value;
            }
        }

        public event EventHandler<SettablePhotoResult> Completed;

        private void AttachToEvents()
        {
            WrappedSubject.Completed += WrappedSubjectCompleted;
        }

        void WrappedSubjectCompleted(object sender, PhotoResult e)
        {
            CompletedRelay(sender, new SettablePhotoResult(e));
        }

        public void Show()
        {
            WrappedSubject.Show();
        }

        private void CompletedRelay(object sender, SettablePhotoResult e)
        {
            var handler = Completed;
            if (handler != null)
            {
                handler(sender, e);
            }
        }
    }
}
