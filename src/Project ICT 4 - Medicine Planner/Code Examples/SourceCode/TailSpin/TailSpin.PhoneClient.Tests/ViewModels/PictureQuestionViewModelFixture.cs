//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TailSpin.Phone.TestSupport;
using TailSpin.PhoneClient.Adapters;
using TailSpin.PhoneClient.ViewModels;
using TailSpin.PhoneServices.Models;

namespace TailSpin.PhoneClient.Tests.ViewModels
{
    [TestClass]
    public class PictureQuestionViewModelFixture : SilverlightTest
    {
        [TestMethod, Asynchronous]
        public void ViewModelGetsPictureFromCameraTask()
        {
            var mockCameraCaptureTask = new MockCameraCaptureTask();
            WriteableBitmap picture = null;
            var imageUri = new Uri("/TailSpin.PhoneClient.Tests;component/ViewModels/Images/block.jpg",
                                    UriKind.Relative);
            var src = new BitmapImage();
            src.SetSource(Application.GetResourceStream(imageUri).Stream);
            picture = new WriteableBitmap(src);

            var sri = Application.GetResourceStream(imageUri);
            mockCameraCaptureTask.TaskEventArgs =
                new SettablePhotoResult { ChosenPhoto = sri.Stream };
            
            var questionAnswer = new QuestionAnswer { QuestionText = "Will this test pass?" };

            var target = new PictureQuestionViewModel(questionAnswer, mockCameraCaptureTask, new MockMessageBox());
            Assert.IsNull(target.Picture);

            target.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName != "Picture") return;

                Assert.IsNotNull(target.Picture);
                Assert.IsTrue(IntArraysMatch(target.Picture.Pixels, picture.Pixels));

                EnqueueTestComplete();
            };

            target.CameraCaptureCommand.Execute();
        }

        [TestMethod, Asynchronous]
        public void ViewModelReEnablesCommandIfCameraTaskCancels()
        {
            var mockCameraCaptureTask = new MockCameraCaptureTask();

            mockCameraCaptureTask.TaskEventArgs = new SettablePhotoResult();

            var questionAnswer = new QuestionAnswer { QuestionText = "Will this test pass?" };

            var target = new PictureQuestionViewModel(questionAnswer, mockCameraCaptureTask, new MessageBoxAdapter());

            target.CameraCaptureCommand.CanExecuteChanged += (s, e) =>
            {
                if (target.Capturing == false)
                {
                    EnqueueTestComplete();                    
                }
            };

            target.CameraCaptureCommand.Execute();
        }

        [TestMethod, Asynchronous]
        public void ViewModelShowsErrorMessageIfCameraTaskReturnsError()
        {
            var mockCameraCaptureTask = new MockCameraCaptureTask();
            var mockMessageBox = new MockMessageBox();

            mockCameraCaptureTask.TaskEventArgs = new SettablePhotoResult { Error = new Exception("test exception message") };

            var questionAnswer = new QuestionAnswer { QuestionText = "Will this test pass?" };

            var target = new PictureQuestionViewModel(questionAnswer, mockCameraCaptureTask, mockMessageBox);

            target.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName != "Capturing" && target.Capturing != false) return;

                Assert.AreEqual("test exception message", mockMessageBox.MessageBoxText);

                EnqueueTestComplete();
            };

            target.CameraCaptureCommand.Execute();
        }

        public bool IntArraysMatch(int[] a, int[] b)
        {
            return a.Length == b.Length && !a.Where((t, i) => t != b[i]).Any();
        }
    }


}
