//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Shell;
using Microsoft.Practices.Prism.Commands;
using TailSpin.Phone.Adapters;
using TailSpin.PhoneClient.Adapters;
using TailSpin.PhoneServices.Models;
using TailSpin.PhoneServices.Services;
using TailSpin.PhoneServices.Services.Stores;
using TailSpin.PhoneClient.Services;

namespace TailSpin.PhoneClient.ViewModels
{
    public class TakeSurveyViewModel : ViewModel
    {
        private static readonly Dictionary<QuestionType, Func<QuestionAnswer, QuestionViewModel>> Maps =
            new Dictionary<QuestionType, Func<QuestionAnswer, QuestionViewModel>>
                {
                    { QuestionType.SimpleText, a => new OpenQuestionViewModel(a) }, 
                    { QuestionType.MultipleChoice, a => new MultipleChoiceQuestionViewModel(a) }, 
                    { QuestionType.FiveStars, a => new FiveStarsQuestionViewModel(a) }, 
                    { QuestionType.Voice, a => new VoiceQuestionViewModel(a, new IsolatedStorageFacade(), new ApplicationFrameNavigationService(((App)Application.Current).RootFrame)) }, 
                    { QuestionType.Picture, a => new PictureQuestionViewModel(a, new CameraCaptureTaskAdapter(), new MessageBoxAdapter()) }
                };

        private ILocationService locationService;
        private readonly IShellTile shellTile;
        private SurveyAnswer surveyAnswer;
        private ISurveyStoreLocator surveyStoreLocator;
        private SurveyTemplateViewModel templateViewModel;
        private SurveyAnswer tombstoned;
        private DelegateCommand completeCommand;
        private DelegateCommand pinCommand;
        private DelegateCommand saveCommand;
        private List<QuestionAnswer> cleanAnswers;
        private string surveyId;
        private bool isProcessing;
        private int selectedPivotIndex;

        public TakeSurveyViewModel(
            INavigationService navigationService,
            IPhoneApplicationServiceFacade phoneApplicationServiceFacade,
            ILocationService locationService,
            ISurveyStoreLocator surveyStoreLocator,
            IShellTile shellTile)
            : base(navigationService, phoneApplicationServiceFacade, new Uri("/Views/TakeSurvey/TakeSurveyView.xaml", UriKind.Relative))
        {
            this.locationService = locationService;
            this.surveyStoreLocator = surveyStoreLocator;
            this.shellTile = shellTile;
        }

        public DelegateCommand CompleteCommand
        {
            get { return completeCommand; }
            set
            {
                completeCommand = value;
                RaisePropertyChanged(() => this.CompleteCommand);
            }
        }

        public DelegateCommand PinCommand
        {
            get { return pinCommand; }
            set
            {
                pinCommand = value;
                RaisePropertyChanged(() => this.PinCommand);
            }
        }

        public DelegateCommand SaveCommand
        {
            get { return saveCommand; }
            set
            {
                saveCommand = value;
                RaisePropertyChanged(() => this.SaveCommand);
            }
        }

        public IList<QuestionViewModel> Questions { get; set; }

        public int SelectedPivotIndex
        {
            get { return selectedPivotIndex; }
            set
            {
                if (Questions != null)
                {
                    var voiceQuestionViewModel = Questions[selectedPivotIndex] as VoiceQuestionViewModel;
                    if (voiceQuestionViewModel != null)
                    {
                        StopVoiceRecordingAndPlayback(voiceQuestionViewModel);
                    }
                }

                selectedPivotIndex = value;
            }
        }

        public SurveyAnswer SurveyAnswer
        {
            get { return this.surveyAnswer; }

            set
            {
                // Set only if there was not a value coming from tombstoning
                if (this.surveyAnswer == null)
                {
                    this.surveyAnswer = value;
                    if (this.tombstoned != null)
                    {
                        // There was a tombstoned one
                        this.surveyAnswer.SetAnswersFrom(this.tombstoned);
                    }
                }

                this.SubscribeToAnswersChanged();
            }
        }

        public SurveyTemplateViewModel TemplateViewModel
        {
            get { return this.templateViewModel; }

            set
            {
                this.templateViewModel = value;
            }
        }

        public bool IsProcessing
        {
            get { return this.isProcessing; }
            set
            {
                this.isProcessing = value;
                this.RaisePropertyChanged(() => this.IsProcessing);
            }
        }


        public override sealed void OnPageResumeFromTombstoning()
        {
            this.tombstoned = this.PhoneApplicationServiceFacade.Load<SurveyAnswer>("TakeSurveyAnswer");
            this.SelectedPivotIndex = this.PhoneApplicationServiceFacade.Load<int>("TakeSurveyCurrentIndex");
            this.surveyId = this.PhoneApplicationServiceFacade.Load<string>("TakeSurveyId");
            Initialize(this.surveyId);
            this.locationService.StartWatcher();
        }

        public override void OnPageDeactivation(bool isIntentionalNavigation)
        {
            base.OnPageDeactivation(isIntentionalNavigation);

            if (isIntentionalNavigation)
            {
                this.DisposeControls();
                this.Dispose();
                return;
            }

            this.PhoneApplicationServiceFacade.Save("TakeSurveyAnswer", this.SurveyAnswer);
            this.PhoneApplicationServiceFacade.Save("TakeSurveyCurrentIndex", this.SelectedPivotIndex);
            this.PhoneApplicationServiceFacade.Save("TakeSurveyId", this.surveyId);
        }

        private void StopAllVoiceRecordingsAndPlaybacks()
        {
            foreach (var questionViewModel in Questions)
            {
                var voiceQuestionViewModel = questionViewModel as VoiceQuestionViewModel;
                if (voiceQuestionViewModel != null)
                {
                    StopVoiceRecordingAndPlayback(voiceQuestionViewModel);
                }
            }
        }

        private void StopVoiceRecordingAndPlayback(VoiceQuestionViewModel voiceQuestionViewModel)
        {
            if (voiceQuestionViewModel.IsRecording)
                voiceQuestionViewModel.DefaultActionCommand.Execute(null);

            if (voiceQuestionViewModel.IsPlaying)
                voiceQuestionViewModel.StopPlayback();
        }

        public void Initialize(string surveyID)
        {
            this.surveyId = surveyID;
            var surveyTemplate = this.surveyStoreLocator.GetStore().GetSurveyTemplates().Single(s => s.SlugName == surveyID);
            if (surveyTemplate != null)
                Initialize(new SurveyTemplateViewModel(surveyTemplate, this.NavigationService, this.PhoneApplicationServiceFacade, this.shellTile, this.locationService));
        }

        private void Initialize(SurveyTemplateViewModel surveyTemplateViewModel)
        {
            this.locationService.StartWatcher();

            this.IsProcessing = true;
            this.TemplateViewModel = surveyTemplateViewModel;

            if (TemplateViewModel.Template.IsNew)
            {
                surveyStoreLocator.GetStore().MarkSurveyTemplateRead(TemplateViewModel.Template);
            }

            SurveyAnswer = surveyStoreLocator.GetStore().GetCurrentAnswerForTemplate(TemplateViewModel.Template)
                            ?? TemplateViewModel.Template.CreateSurveyAnswers();

            //save clean copy of initial answers
            cleanAnswers = new List<QuestionAnswer>();
            foreach (var a in SurveyAnswer.Answers)
            {
                cleanAnswers.Add(new QuestionAnswer() { Value = a.Value });
            }

            this.SaveCommand =
                new DelegateCommand(
                    () => SaveSurvey(true),
                    () => this.surveyAnswer.CompletenessPercentage > 0);

            this.PinCommand =
                new DelegateCommand(
                    () => TemplateViewModel.PinToStart(),
                    () => TemplateViewModel.IsPinnable);

            this.CompleteCommand =
                new DelegateCommand(
                    () =>
                    {
                        this.IsProcessing = true;
                        this.surveyAnswer.IsComplete = true;
                        this.surveyAnswer.CompleteLocation = this.locationService.TryToGetCurrentLocation();
                        this.surveyStoreLocator.GetStore().SaveSurveyAnswer(this.SurveyAnswer);
                        this.TemplateViewModel.AnswersQty++;
                        this.TemplateViewModel.Completeness = 0;
                        this.IsProcessing = false;
                        this.CleanUpAndGoBack(true);
                    },
                    () => this.surveyAnswer.CompletenessPercentage == 100);

            this.Questions = this.SurveyAnswer.Answers.Select(a => Maps[a.QuestionType].Invoke(a)).ToArray();

            //Populate Header Titles
            for (var i = 0; i < this.Questions.Count; i++)
            {
                var questionViewModel = this.Questions[i];
                questionViewModel.Title = string.Format("question{0}", i + 1);
            }

            SurveyAnswer.StartLocation = locationService.TryToGetCurrentLocation();

            this.IsProcessing = false;
        }

        public void SaveDirtyDataAndCleanUp()
        {
            SaveSurvey(false);
            CleanUp();
        }

        private void AnswerChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            this.TemplateViewModel.Completeness = this.SurveyAnswer.CompletenessPercentage;
            this.TemplateViewModel.CompletedAnswers = this.SurveyAnswer.CompletedAnswers;
            this.CompleteCommand.RaiseCanExecuteChanged();
            this.SaveCommand.RaiseCanExecuteChanged();
        }

        private void SaveSurvey(bool autoCleanUp)
        {
            StopAllVoiceRecordingsAndPlaybacks();

            this.surveyStoreLocator.GetStore().SaveSurveyAnswer(this.SurveyAnswer);
            this.TemplateViewModel.Completeness = this.SurveyAnswer.CompletenessPercentage;
            if (autoCleanUp)
                this.CleanUpAndGoBack(false);
        }

        private void CleanUpAndGoBack(bool completed)
        {
            if (NavigationService.CanGoBack)
            {
                CleanUp();
                this.NavigationService.GoBack();
            }
            else if (completed)
            {
                //working from a pinned tile state and complete was tapped
                MessageBox.Show("This survey has been saved and will be uploaded to the Tailspin service during the next synchronization.", "Survey Completed", MessageBoxButton.OK);
                this.surveyAnswer = null;
                this.tombstoned = null;
                this.Questions = null; //done to reset the pivot index to 0
                RaisePropertyChanged(() => this.Questions);
                Initialize(this.TemplateViewModel);
                RaisePropertyChanged(() => this.Questions);
            }
            else
            {
                //working from a pinned tile state and save was tapped
                MessageBox.Show("This survey has been saved.", "Survey Saved", MessageBoxButton.OK);
            }

        }

        private void CleanUp()
        {
            this.PhoneApplicationServiceFacade.Remove("TakeSurveyAnswer");
            this.PhoneApplicationServiceFacade.Remove("TakeSurveyCurrentIndex");
            this.PhoneApplicationServiceFacade.Remove("TakeSurveyId");

            this.UnsubscribeToAnswersChanged();
            this.locationService.StopWatcher();
        }

        private void DisposeControls()
        {
            foreach (var questionViewModel in this.Questions)
            {
                var disposable = questionViewModel as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        private void SubscribeToAnswersChanged()
        {
            this.surveyAnswer.Answers.ForEach(a => a.PropertyChanged += this.AnswerChangedHandler);
        }

        private void UnsubscribeToAnswersChanged()
        {
            this.surveyAnswer.Answers.ForEach(a => a.PropertyChanged -= this.AnswerChangedHandler);
        }
    }
}