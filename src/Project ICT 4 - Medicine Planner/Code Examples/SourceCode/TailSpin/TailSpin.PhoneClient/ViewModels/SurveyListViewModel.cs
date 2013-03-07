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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Microsoft.Phone.Reactive;
using Microsoft.Phone.Shell;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using TailSpin.PhoneClient.Adapters;
using TailSpin.PhoneServices.Services;
using TailSpin.PhoneServices.Services.Stores;
using InteractionRequest = Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace TailSpin.PhoneClient.ViewModels
{
    public class SurveyListViewModel : ViewModel
    {
        private readonly InteractionRequest<InteractionRequest.Notification> submitErrorInteractionRequest;
        private readonly InteractionRequest<InteractionRequest.Notification> submitNotificationInteractionRequest;
        private readonly ISurveyStoreLocator surveyStoreLocator;
        private readonly ISurveysSynchronizationService synchronizationService;
        private readonly IShellTile shellTile;
        private CollectionViewSource bylengthViewSource;
        private CollectionViewSource favoritesViewSource;
        private ISurveyStore lastSurveyStore;
        private ISettingsStore settingsStore;
        private readonly ILocationService locationService;
        private CollectionViewSource newSurveysViewSource;
        private int selectedPivotIndex;
        private SurveyTemplateViewModel selectedSurveyTemplate;
        private bool isSyncing;

        public SurveyListViewModel(
            ISurveyStoreLocator surveyStoreLocator,
            ISurveysSynchronizationService synchronizationService,
            INavigationService navigationService,
            IPhoneApplicationServiceFacade phoneApplicationServiceFacade,
            IShellTile shellTile,
            ISettingsStore settingsStore,
            ILocationService locationService)
            : base(navigationService, phoneApplicationServiceFacade, new Uri(@"/Views/SurveyList/SurveyListView.xaml", UriKind.Relative))
        {
            this.surveyStoreLocator = surveyStoreLocator;
            this.synchronizationService = synchronizationService;
            this.shellTile = shellTile;
            this.submitErrorInteractionRequest =
                new InteractionRequest<InteractionRequest.Notification>();

            this.submitNotificationInteractionRequest =
                new InteractionRequest<InteractionRequest.Notification>();
            this.submitErrorInteractionRequest = new InteractionRequest<InteractionRequest.Notification>();
            this.submitNotificationInteractionRequest = new InteractionRequest<InteractionRequest.Notification>();

            this.StartSyncCommand = new DelegateCommand(
                this.StartSync,
                () => !this.IsSynchronizing && !this.SettingAreNotConfigured);

            this.FiltersCommand = new DelegateCommand(
                () => this.NavigationService.Navigate(new Uri("/Views/FilterSettingsView.xaml", UriKind.Relative)),
                () => !this.IsSynchronizing && !this.SettingAreNotConfigured);

            this.AppSettingsCommand = new DelegateCommand(
                () => this.NavigationService.Navigate(new Uri("/Views/AppSettingsView.xaml", UriKind.Relative)),
                () => !this.IsSynchronizing);
            
            this.settingsStore = settingsStore;
            this.locationService = locationService;
            this.settingsStore.UserChanged += settingsStore_UserChanged;

            if (SettingAreConfigured)
            {
                Refresh();
            }
        }

        void settingsStore_UserChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        public DelegateCommand AppSettingsCommand { get; set; }

        public ObservableCollection<SurveyTemplateViewModel> ObservableSurveyTemplates { get; set; }

        public ICollectionView ByLengthItems
        {
            get { return this.bylengthViewSource.View; }
        }

        public ICollectionView FavoriteItems
        {
            get { return this.favoritesViewSource.View; }
        }

        public DelegateCommand FiltersCommand { get; set; }

        public bool IsSynchronizing
        {
            get { return this.isSyncing; }
            set
            {
                this.isSyncing = value;
                this.RaisePropertyChanged(() => this.IsSynchronizing);
                this.StartSyncCommand.RaiseCanExecuteChanged();
                this.FiltersCommand.RaiseCanExecuteChanged();
                this.AppSettingsCommand.RaiseCanExecuteChanged();
            }
        }

        public ICollectionView NewItems
        {
            get { return this.newSurveysViewSource.View; }
        }

        public int SelectedPivotIndex
        {
            get { return this.selectedPivotIndex; }

            set
            {
                this.selectedPivotIndex = value;
                this.HandleCurrentSectionChanged();
            }
        }

        public SurveyTemplateViewModel SelectedSurveyTemplate
        {
            get { return this.selectedSurveyTemplate; }

            set
            {
                if (value != null)
                {
                    this.selectedSurveyTemplate = value;
                    this.RaisePropertyChanged(() => this.SelectedSurveyTemplate);
                }
            }
        }

        public bool SettingAreConfigured
        {
            get { return !this.SettingAreNotConfigured; }
        }

        public bool SettingAreNotConfigured
        {
            get { return this.surveyStoreLocator.GetStore() is NullSurveyStore; }
        }

        public DelegateCommand StartSyncCommand { get; set; }

        public IInteractionRequest SubmitErrorInteractionRequest
        {
            get { return this.submitErrorInteractionRequest; }
        }

        public IInteractionRequest SubmitNotificationInteractionRequest
        {
            get { return this.submitNotificationInteractionRequest; }
        }

        public override void OnPageDeactivation(bool isIntentionalNavigation)
        {
            this.PhoneApplicationServiceFacade.Save("MainPivot", this.SelectedPivotIndex); 
        }

        public override sealed void OnPageResumeFromTombstoning()
        {
            this.selectedPivotIndex = this.PhoneApplicationServiceFacade.Load<int>("MainPivot");
        }

        public void Refresh()
        {
            if (this.surveyStoreLocator.GetStore() != this.lastSurveyStore)
            {
                this.lastSurveyStore = this.surveyStoreLocator.GetStore();
                this.lastSurveyStore.SurveyAnswerSaved += lastSurveyStore_SurveyAnswerSaved;
            }

            //rebuild UI to reflect updated state
            this.BuildPivotDimensions();
            this.StartSyncCommand.RaiseCanExecuteChanged();
            this.FiltersCommand.RaiseCanExecuteChanged();                
        }

        void lastSurveyStore_SurveyAnswerSaved(object sender, EventArgs e)
        {
            BuildPivotDimensions();
        }

        public void StartSync()
        {
            if (this.IsSynchronizing)
            {
                return;
            }

            this.IsSynchronizing = true;
            this.synchronizationService
                .StartSynchronization()
                .ObserveOnDispatcher()
                .Subscribe(this.SyncCompleted);
        }

        public void ResetUnopenedSurveyCount()
        {
            synchronizationService.UnopenedSurveyCount = 0;
            var tile = shellTile.ActiveTiles.First();
            var tileData = new StandardTileData()
            {
                Count = 0
            };
            tile.Update(tileData);
        }

        private void BuildPivotDimensions()
        {
            this.ObservableSurveyTemplates = new ObservableCollection<SurveyTemplateViewModel>();
            List<SurveyTemplateViewModel> surveyTemplateViewModels =
                this.surveyStoreLocator.GetStore().GetSurveyTemplates().Select(t => new SurveyTemplateViewModel(t, this.NavigationService, this.PhoneApplicationServiceFacade, this.shellTile, this.locationService)
                    {
                            CompletedAnswers = this.surveyStoreLocator.GetStore().GetCurrentAnswerForTemplate(t) != null
                                                ? this.surveyStoreLocator.GetStore().GetCurrentAnswerForTemplate(t).CompletedAnswers : 0,
                            Completeness = this.surveyStoreLocator.GetStore().GetCurrentAnswerForTemplate(t) != null
                                                ? this.surveyStoreLocator.GetStore().GetCurrentAnswerForTemplate(t).CompletenessPercentage : 0,
                            CanTakeSurvey = () => !IsSynchronizing

                    }).ToList();
            surveyTemplateViewModels.ForEach(this.ObservableSurveyTemplates.Add);

            // Listen for survey changes
            // and calculate answersQty
            this.ListenSurveyChanges();

            // Create collection views
            this.newSurveysViewSource = new CollectionViewSource { Source = this.ObservableSurveyTemplates };
            this.bylengthViewSource = new CollectionViewSource { Source = this.ObservableSurveyTemplates };
            this.favoritesViewSource = new CollectionViewSource { Source = this.ObservableSurveyTemplates };

            this.newSurveysViewSource.Filter += (o, e) => e.Accepted = ((SurveyTemplateViewModel)e.Item).Template.IsNew;
            this.favoritesViewSource.Filter += (o, e) => e.Accepted = ((SurveyTemplateViewModel)e.Item).Template.IsFavorite;
            this.bylengthViewSource.SortDescriptions.Add(new SortDescription("Length", ListSortDirection.Ascending));

            this.newSurveysViewSource.View.CurrentChanged += 
                (o, e) => this.SelectedSurveyTemplate = (SurveyTemplateViewModel)this.newSurveysViewSource.View.CurrentItem;
            this.favoritesViewSource.View.CurrentChanged += 
                (o, e) => this.SelectedSurveyTemplate = (SurveyTemplateViewModel)this.favoritesViewSource.View.CurrentItem;
            this.bylengthViewSource.View.CurrentChanged += 
                (o, e) => this.SelectedSurveyTemplate = (SurveyTemplateViewModel)this.bylengthViewSource.View.CurrentItem;

            // Initialize the selected survey template
            this.HandleCurrentSectionChanged();

            this.RaisePropertyChanged(string.Empty);
        }

        private void HandleCurrentSectionChanged()
        {
            ICollectionView currentView = null;
            switch (this.SelectedPivotIndex)
            {
                case 0:
                    currentView = this.NewItems;
                    break;
                case 1:
                    currentView = this.FavoriteItems;
                    break;
                case 2:
                    currentView = this.ByLengthItems;
                    break;
            }

            if (currentView != null)
            {
                this.SelectedSurveyTemplate = (SurveyTemplateViewModel)currentView.CurrentItem;
            }
        }

        private void ListenSurveyChanges()
        {
            foreach (var template in this.ObservableSurveyTemplates)
            {
                template.PropertyChanged += (s, e) =>
                                                {
                                                    if (e.PropertyName == "IsFavorite")
                                                    {
                                                        this.favoritesViewSource.View.Refresh();
                                                    }
                                                };

                // Calculate Answers Qty
                SurveyTemplateViewModel templateCopy = template;
                template.AnswersQty = this.surveyStoreLocator.GetStore()
                    .GetCompleteSurveyAnswers()
                    .Count(a =>
                           a.Tenant == templateCopy.Tenant &&
                           a.SlugName == templateCopy.Template.SlugName);
            }
        }

        private void SyncCompleted(IEnumerable<TaskCompletedSummary> taskSummaries)
        {
            var stringBuilder = new StringBuilder();

            var errorSummary =
                taskSummaries.FirstOrDefault(
                    s =>
                        s.Result == TaskSummaryResult.UnreachableServer ||
                        s.Result == TaskSummaryResult.AccessDenied);

            if (errorSummary != null)
            {
                stringBuilder.AppendLine(TaskCompletedSummaryStrings.GetDescriptionForSummary(errorSummary));
                this.submitErrorInteractionRequest.Raise(
                        new InteractionRequest.Notification
                            {
                                Title = "Synchronization error", Content = stringBuilder.ToString()
                            },
                        n => { });
            }
            else
            {
                foreach (var task in taskSummaries)
                {
                    stringBuilder.AppendLine(TaskCompletedSummaryStrings.GetDescriptionForSummary(task));
                }

                if (taskSummaries.Any(t => t.Result != TaskSummaryResult.Success))
                {
                    this.submitErrorInteractionRequest.Raise(
                        new InteractionRequest.Notification
                            {
                                Title = "Synchronization error", Content = stringBuilder.ToString()
                            },
                        n => { });
                }
                else
                {
                    this.submitNotificationInteractionRequest.Raise(
                        new InteractionRequest.Notification
                            {
                                Title = stringBuilder.ToString(), Content = null
                            },
                        n => { });
                }
            }

            // Update the View Model status
            this.BuildPivotDimensions();
            this.IsSynchronizing = false;
            this.UpdateCommandsForSync();
        }

        private void UpdateCommandsForSync()
        {
            this.StartSyncCommand.RaiseCanExecuteChanged();
            this.FiltersCommand.RaiseCanExecuteChanged();
            this.AppSettingsCommand.RaiseCanExecuteChanged();
        }
    }
}
