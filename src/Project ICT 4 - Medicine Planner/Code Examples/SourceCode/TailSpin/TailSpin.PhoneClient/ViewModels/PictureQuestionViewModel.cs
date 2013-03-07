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
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using Microsoft.Phone;
using Microsoft.Phone.Reactive;
using Microsoft.Practices.Prism.Commands;
using TailSpin.PhoneClient.Adapters;
using TailSpin.PhoneServices.Models;

namespace TailSpin.PhoneClient.ViewModels
{
    public class PictureQuestionViewModel : QuestionViewModel
    {
        private readonly ICameraCaptureTask task;
        private bool capturing, savingPictureFile;
        private WriteableBitmap picture;
        private readonly IMessageBox messageBox;
        private readonly string fileName;
        
        public PictureQuestionViewModel(QuestionAnswer questionAnswer, ICameraCaptureTask cameraCaptureTask, IMessageBox messageBox)
            : base(questionAnswer)
        {
            this.CameraCaptureCommand = new DelegateCommand(this.CapturePicture, () => !this.Capturing);
            if (questionAnswer.Value != null)
            {
                this.LoadPictureBitmap(questionAnswer.Value);
            }
            this.task = cameraCaptureTask;
            this.messageBox = messageBox;

            fileName = string.Format(CultureInfo.InvariantCulture, "{0}.jpeg", Guid.NewGuid());
            
            // Subscribe to handle new photo stream
            Observable.FromEvent<SettablePhotoResult>(h => this.task.Completed += h, h => this.task.Completed -= h)
                .Where(e => e.EventArgs.ChosenPhoto != null)
                .Subscribe(result =>
                               {
                                   this.Capturing = false;
                                   SavePictureFile(result.EventArgs.ChosenPhoto);
                                   this.Answer.Value = this.fileName;
                                   this.RaisePropertyChanged(string.Empty);
                                   this.CameraCaptureCommand.RaiseCanExecuteChanged();
                               });

            // Subscribe to user cancelling photo capture to re-enable Capture command
            Observable.FromEvent<SettablePhotoResult>(h => this.task.Completed += h, h => this.task.Completed -= h)
                .Where(e => e.EventArgs.ChosenPhoto == null && e.EventArgs.Error == null)
                .Subscribe(p =>
                                {
                                    this.Capturing = false;
                                    this.CameraCaptureCommand.RaiseCanExecuteChanged();
                                });

            // Subscribe to Error condition
            Observable.FromEvent<SettablePhotoResult>(h => this.task.Completed += h, h => this.task.Completed -= h)
                .Where(e => e.EventArgs.Error != null && !string.IsNullOrEmpty(e.EventArgs.Error.Message))
                .Subscribe(p =>
                                {
                                    this.Capturing = false;
                                    this.messageBox.Show(p.EventArgs.Error.Message);
                                    this.CameraCaptureCommand.RaiseCanExecuteChanged();
                                });
        }

        public DelegateCommand CameraCaptureCommand { get; set; }

        public WriteableBitmap Picture
        {
            get { return this.picture; }
            set
            {
                if (this.picture != value)
                {
                    this.picture = value;
                    this.RaisePropertyChanged(() => this.Picture);
                }
            }
        }

        public bool Capturing
        {
            get { return this.capturing; }
            set
            {
                if (this.capturing != value)
                {
                    this.capturing = value;
                    this.RaisePropertyChanged(() => this.Capturing);
                }
            }
        }

        public bool SavingPictureFile
        {
            get { return this.savingPictureFile; }
            set
            {
                if (this.savingPictureFile != value)
                {
                    this.savingPictureFile = value;
                    this.RaisePropertyChanged(() => this.SavingPictureFile);
                }
            }
        }

        private void SavePictureFile(Stream chosenPhoto)
        {
            SavingPictureFile = true;

            // Store the image bytes.
            byte[] imageBytes = new byte[chosenPhoto.Length];
            chosenPhoto.Read(imageBytes, 0, imageBytes.Length);

            // Seek back so we can create an image.
            chosenPhoto.Seek(0, SeekOrigin.Begin);

            // Create an image from the stream.
            var imageSource = PictureDecoder.DecodeJpeg(chosenPhoto);
            this.Picture = imageSource;

            // Save the stream

            var isoFile = IsolatedStorageFile.GetUserStoreForApplication();
            using (var stream = isoFile.CreateFile(fileName))
            {
                stream.Write(imageBytes, 0, imageBytes.Length);
            }

            SavingPictureFile = false;
        }

        private void LoadPictureBitmap(string path)
        {
            using (var filesystem = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (filesystem.FileExists(path))
                {
                    using (var fs = new IsolatedStorageFileStream(path, FileMode.Open, filesystem))
                    {
                        this.Picture = PictureDecoder.DecodeJpeg(fs);
                    }
                }
            }
        }

        private void CapturePicture()
        {
            if (!this.Capturing)
            {
                this.task.Show();
                this.Capturing = true;
                this.CameraCaptureCommand.RaiseCanExecuteChanged();
            }
        }
    }
}