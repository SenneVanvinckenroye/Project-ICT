//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Reactive;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using TailSpin.Phone.Adapters;
using TailSpin.PhoneClient.Adapters;
using TailSpin.PhoneClient.Infrastructure;
using TailSpin.PhoneServices.Models;

namespace TailSpin.PhoneClient.ViewModels
{
    public class VoiceQuestionViewModel : QuestionViewModel, IDisposable
    {
        private readonly string wavFileName;
        private WaveFormatter formatter;
        private bool isPlaying;
        private IDisposable observableMic;
        private byte[] buffer;
        private MemoryStream stream;
        private XnaAsyncDispatcher xnaAsyncDispatcher;
        private IIsolatedStorageFacade isolatedStorageFacade;
        private INavigationService navigationService;
        private MediaState priorMediaState;
        private IsolatedStorageFile fileSystem;
        private IsolatedStorageFileStream fileStream;
        private SoundEffect soundEffect;
        private SoundEffectInstance soundEffectInstance;

        public VoiceQuestionViewModel(QuestionAnswer questionAnswer, IIsolatedStorageFacade isolatedStorageFacade, INavigationService navigationService)
            : base(questionAnswer)
        {
            this.DefaultActionCommand = new DelegateCommand(this.DefaultAction);
            this.wavFileName = questionAnswer.Value ?? string.Format("{0}.wav", Guid.NewGuid());
            this.PlayCommand = 
                new DelegateCommand(this.Play, () => !this.IsPlaying && !this.IsRecording && !string.IsNullOrEmpty(this.Answer.Value));
            
            // start the XNA Dispatcher
            xnaAsyncDispatcher = new XnaAsyncDispatcher(TimeSpan.FromMilliseconds(50));
            xnaAsyncDispatcher.StartService();
            this.isolatedStorageFacade = isolatedStorageFacade;
            this.navigationService = navigationService;
            this.navigationService.Obscured += navigationService_Obscured;
        }

        void navigationService_Obscured(object sender, ObscuredEventArgs e)
        {
            if (!IsRecording) return;

            this.StopRecording();
            this.IsRecording = false;
            this.PlayCommand.RaiseCanExecuteChanged();
            this.RaisePropertyChanged(string.Empty);
        }

        public ICommand DefaultActionCommand { get; set; }

        public string DefaultActionText
        {
            get { return this.IsRecording ? "Stop Recording" : "Start Recording"; }
        }

        public bool IsPlaying
        {
            get
            {
                if (soundEffectInstance != null)
                {
                    return soundEffectInstance.State == SoundState.Playing;
                }
                return false;
            }
        }

        public bool IsRecording { get; set; }

        public DelegateCommand PlayCommand { get; set; }

        public void Dispose()
        {
            if (this.IsRecording)
            {
                this.StopRecording();

                //reset question for the case of returning from dormant state
                this.IsRecording = false;
                this.Answer.Value = null;
                this.PlayCommand.RaiseCanExecuteChanged();
                this.RaisePropertyChanged(string.Empty);
            }
            xnaAsyncDispatcher.StopService();
            this.navigationService.Obscured -= navigationService_Obscured;
            if (soundEffectInstance != null) soundEffectInstance.Dispose();
            if (soundEffect != null) soundEffect.Dispose();
            if (fileStream != null) fileStream.Dispose();
            if (fileSystem != null) fileSystem.Dispose();
        }

        private void DefaultAction()
        {
            if (!this.IsRecording)
            {
                this.StartRecording();
            }
            else
            {
                this.StopRecording();
            }

            this.IsRecording = !this.IsRecording;
            this.PlayCommand.RaiseCanExecuteChanged();
            this.RaisePropertyChanged(string.Empty);
        }

        private void Play()
        {
            priorMediaState = MediaPlayer.State;
            if (priorMediaState == MediaState.Playing)
            {
                MediaPlayer.Pause();
            }

            fileSystem = IsolatedStorageFile.GetUserStoreForApplication();

            fileStream = fileSystem.OpenFile(this.wavFileName, FileMode.Open, FileAccess.Read);

            try
            {
                soundEffect = SoundEffect.FromStream(fileStream);
                soundEffectInstance = soundEffect.CreateInstance();
                soundEffectInstance.Play();
            }
            catch (ArgumentException)
            {
            }

            if (priorMediaState == MediaState.Playing)
            {
                MediaPlayer.Resume();
            }
        }

        public void StopPlayback()
        {
            if (soundEffectInstance == null) return;

            if (soundEffectInstance.State == SoundState.Playing)
            {
                soundEffectInstance.Stop(true);
            }
        }

        private void StartRecording()
        {
            StopPlayback();

            priorMediaState = MediaPlayer.State;
            if (priorMediaState == MediaState.Playing)
            {
                MediaPlayer.Pause();
            }

            var mic = Microphone.Default;
            if (mic.State == MicrophoneState.Started)
            {
                mic.Stop();
            }

            this.stream = new MemoryStream();

            buffer = new byte[mic.GetSampleSizeInBytes(mic.BufferDuration)];

            this.observableMic = Observable.FromEvent<EventArgs>(h => mic.BufferReady += h, h => mic.BufferReady -= h)
                .Subscribe(p => this.CaptureMicrophoneBufferResults());
            mic.Start();
        }

        private void CaptureMicrophoneBufferResults()
        {
            if (this.buffer == null) return;
            
            var mic = Microphone.Default;
            
            int bufferCaptured = mic.GetData(this.buffer);
            this.stream.Write(this.buffer, 0, bufferCaptured);
        }

        /// <summary>
        /// This StopRecording method is necessary to save the captured voice data stream into a wave file.
        /// If this method is not called, the voice recording will not be saved to isolated storage.
        /// For example, if the survey is saved without explicitly stopping the voice recording,
        /// this method will not be called.
        /// </summary>
        private void StopRecording()
        {
            var mic = Microphone.Default;
            this.CaptureMicrophoneBufferResults(); // Capture last bit of buffer not capture by event.
            mic.Stop();

            this.formatter = new WaveFormatter(this.wavFileName, (ushort)mic.SampleRate, 16, 1, isolatedStorageFacade);
            this.formatter.WriteDataChunk(stream.ToArray());
           
            this.stream.Dispose();
            this.observableMic.Dispose();
            this.formatter.Dispose();
            this.formatter = null;
            this.buffer = null;
            this.Answer.Value = this.wavFileName;

            if (priorMediaState == MediaState.Playing)
            {
                MediaPlayer.Resume();
            }
        }
    }
}