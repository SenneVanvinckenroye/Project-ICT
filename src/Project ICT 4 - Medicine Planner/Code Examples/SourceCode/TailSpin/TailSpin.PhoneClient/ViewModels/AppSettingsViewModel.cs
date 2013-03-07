//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using System.Windows;
using TailSpin.PhoneClient.Adapters;
using TailSpin.PhoneClient.Services;
using TailSpin.PhoneClient.Services.RegistrationService;
using TailSpin.PhoneServices.Services;
using TailSpin.PhoneServices.Services.Stores;
using System.Net;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Phone.Reactive;
using Notification = Microsoft.Practices.Prism.Interactivity.InteractionRequest.Notification;

namespace TailSpin.PhoneClient.ViewModels
{
    public class AppSettingsViewModel : ViewModel
    {
        private readonly IRegistrationServiceClient registrationServiceClient;
        private readonly ISettingsStore settingsStore;
        private readonly InteractionRequest<Notification> submitErrorInteractionRequest;
        private readonly IScheduledActionClient scheduledActionClient;
        private readonly IMessageBox messageBox;
        private bool canSubmit;
        private bool isSyncing;
        private bool locationServiceAllowed;
        private bool backgroundTasksAllowed;
        private string password;
        private bool subscribeToPushNotifications;
        private string userName;
        private string progressText;
        private IDisposable subscription;

        public AppSettingsViewModel(
            ISettingsStore settingsStore,
            IRegistrationServiceClient registrationServiceClient,
            INavigationService navigationService,
            IPhoneApplicationServiceFacade phoneApplicationServiceFacade,
            IScheduledActionClient scheduledActionClient,
            IMessageBox messageBox)
            : base(navigationService, phoneApplicationServiceFacade, new Uri("/Views/AppSettingsView.xaml", UriKind.Relative))
        {
            this.settingsStore = settingsStore;

            this.registrationServiceClient = registrationServiceClient;
            this.isSyncing = this.registrationServiceClient.IsProcessing;
            this.registrationServiceClient.IsProcessingChanged += registrationServiceClient_IsProcessingChanged;
            
            this.submitErrorInteractionRequest = new InteractionRequest<Notification>();
            this.scheduledActionClient = scheduledActionClient;
            this.messageBox = messageBox;
            this.CancelCommand = new DelegateCommand(this.Cancel);

            this.SubmitCommand = new DelegateCommand(this.Submit, () => this.CanSubmit);

            this.BackgroundTasksAllowed = settingsStore.BackgroundTasksAllowed;
            this.LocationServiceAllowed = settingsStore.LocationServiceAllowed;
            this.UserName = settingsStore.UserName;
            this.Password = settingsStore.Password;
            this.SubscribeToPushNotifications = settingsStore.SubscribeToPushNotifications;
        }

        void registrationServiceClient_IsProcessingChanged(object sender, EventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                                          {
                                                              this.IsSyncing =
                                                                  this.registrationServiceClient.IsProcessing;
                                                              this.CheckCanSubmit();
                                                          });
        }

        public bool BackgroundTasksAllowed
        {
            get { return this.backgroundTasksAllowed; }
            set
            {
                this.backgroundTasksAllowed = value;
                this.RaisePropertyChanged(() => this.BackgroundTasksAllowed);
            }
        }

        public bool CanSubmit
        {
            get { return this.canSubmit; }
            set
            {
                if (!value.Equals(this.canSubmit))
                {
                    this.canSubmit = value;
                    this.RaisePropertyChanged(() => this.CanSubmit);
                    this.SubmitCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public DelegateCommand CancelCommand { get; set; }

        public bool IsSyncing
        {
            get { return this.isSyncing; }
            set
            {
                this.isSyncing = value;
                this.RaisePropertyChanged(() => this.IsSyncing);
            }
        }

        public bool LocationServiceAllowed
        {
            get { return this.locationServiceAllowed; }
            set
            {
                this.locationServiceAllowed = value;
                this.RaisePropertyChanged(() => this.LocationServiceAllowed);
            }
        }

        public bool NetworkAvailable
        {
            get { return NetworkInterface.NetworkInterfaceType != NetworkInterfaceType.None; }
        }

        public string Password
        {
            get { return this.password; }
            set
            {
                if (!string.Equals(value, this.password))
                {
                    this.password = value;
                    this.RaisePropertyChanged(() => this.Password);
                    this.CheckCanSubmit();
                }
            }
        }

        public string ProgressText
        {
            get { return progressText; }
            set
            {
                progressText = value;
                this.RaisePropertyChanged(() => this.ProgressText);
            }
        }

        public DelegateCommand SubmitCommand { get; set; }

        public IInteractionRequest SubmitErrorInteractionRequest
        {
            get { return this.submitErrorInteractionRequest; }
        }

        public bool SubscribeToPushNotifications
        {
            get { return this.subscribeToPushNotifications; }
            set
            {
                this.subscribeToPushNotifications = value;
                this.RaisePropertyChanged(() => this.SubscribeToPushNotifications);
            }
        }

        public string UserName
        {
            get { return this.userName; }
            set
            {
                if (!string.Equals(value, this.userName))
                {
                    this.userName = value;
                    this.RaisePropertyChanged(() => this.UserName);
                    this.CheckCanSubmit();
                }
            }
        }

        public override sealed void OnPageResumeFromTombstoning()
        {
            var tombstonedLocation = this.PhoneApplicationServiceFacade.Load<bool?>("LocationServiceAllowed");
            var tombstonedSubscribe = this.PhoneApplicationServiceFacade.Load<bool?>("SettingsSubscribe");
            var tombstonedBackgroundTasks = this.PhoneApplicationServiceFacade.Load<bool?>("BackgroundTasksAllowed");
            var tombstonedUsername = this.PhoneApplicationServiceFacade.Load<string>("SettingsUsername");
            var tombstonedPassword = this.PhoneApplicationServiceFacade.Load<string>("SettingsPassword");

            if (tombstonedLocation.HasValue)
            {
                this.LocationServiceAllowed = tombstonedLocation.Value;
            }

            if (tombstonedSubscribe.HasValue)
            {
                this.SubscribeToPushNotifications = tombstonedSubscribe.Value;
            }

            if (tombstonedBackgroundTasks.HasValue)
            {
                this.BackgroundTasksAllowed = tombstonedBackgroundTasks.Value;
            }

            if (tombstonedUsername != null)
            {
                this.UserName = tombstonedUsername;
            }

            if (tombstonedPassword != null)
            {
                this.Password = tombstonedPassword;
            }
        }

        public override void OnPageDeactivation(bool isIntentionalNavigation)
        {
            base.OnPageDeactivation(isIntentionalNavigation);

            if (isIntentionalNavigation)
            {
                this.Dispose();
                return;
            }

            if (this.SubscribeToPushNotifications != this.settingsStore.SubscribeToPushNotifications)
            {
                bool? saveVal = this.subscribeToPushNotifications;
                PhoneApplicationServiceFacade.Save("SettingsSubscribe", saveVal);
            }

            if (this.LocationServiceAllowed != this.settingsStore.LocationServiceAllowed)
            {
                bool? saveVal = this.locationServiceAllowed;
                PhoneApplicationServiceFacade.Save("LocationServiceAllowed", saveVal);
            }

            if (this.BackgroundTasksAllowed != this.settingsStore.BackgroundTasksAllowed)
            {
                bool? saveVal = this.backgroundTasksAllowed;
                PhoneApplicationServiceFacade.Save("BackgroundTasksAllowed", saveVal);
            }

            PhoneApplicationServiceFacade.Save("SettingsUsername", this.UserName);
            PhoneApplicationServiceFacade.Save("SettingsPassword", this.Password);
        }

        public void Cancel()
        {
            if (this.NavigationService.CanGoBack) this.NavigationService.GoBack();
        }

        public void Submit()
        {
            this.CanSubmit = false;

            this.settingsStore.UserName = this.UserName;
            this.settingsStore.Password = this.Password;

            //if credentials are not valid
            if (registrationServiceClient.CredentialsAreInvalid())
            {
                messageBox.Show("Invalid username and password combination.", "Invalid Credentials", MessageBoxButton.OK);
                return;
            }

            this.settingsStore.BackgroundTasksAllowed = this.BackgroundTasksAllowed;
            this.settingsStore.LocationServiceAllowed = this.LocationServiceAllowed;

            this.IsSyncing = true;

            //check for discrepency between user preference and OS-level setting
            if (scheduledActionClient.UserEnabled && !scheduledActionClient.IsEnabled)
            {
                messageBox.Show(Constants.DisabledBackgroundMessage, Constants.DisabledBackgroundCaption,
                                MessageBoxButton.OK);
            }

            if (this.SubscribeToPushNotifications == this.settingsStore.SubscribeToPushNotifications)
            {
                this.IsSyncing = false;
                if (this.NavigationService.CanGoBack) this.NavigationService.GoBack();
                return;
            }

            this.ProgressText = "Settings Saved. Updating Push Notifications";

            subscription = this.registrationServiceClient
                .UpdateReceiveNotifications(this.SubscribeToPushNotifications)
                .ObserveOnDispatcher()
                .Subscribe(
            taskSummary =>
            {
                if (taskSummary == TaskSummaryResult.Success)
                {
                    this.settingsStore.SubscribeToPushNotifications =
                        this.SubscribeToPushNotifications;
                    this.IsSyncing = false;
                    if (this.NavigationService.CanGoBack) this.NavigationService.GoBack();
                    if (!SubscribeToPushNotifications)
                    {
                        CleanUp();
                    }
                }
                else
                {
                    // Update unsuccessful, probably due to communication issue with Registration Service.
                    // Don't close channel so that we can retry later.
                    if (SubscribeToPushNotifications)
                    {
                        CleanUp();
                    }
                    var errorText = TaskCompletedSummaryStrings.GetDescriptionForResult(taskSummary);
                    this.IsSyncing = false;
                    this.
                        submitErrorInteractionRequest.Raise(
                            new Notification
                            {
                                Title = "Push Notification: Server error",
                                Content = errorText
                            },
                            n => { });
                }
                this.CanSubmit = true;
            },
            exception =>
            {
                this.CanSubmit = true;
                
                // Update unsuccessful, probably due to communication issue with Registration Service.
                // Don't close channel so that we can retry later.
                if (SubscribeToPushNotifications)
                {
                    CleanUp();
                }

                if (exception is WebException)
                {
                    var webException = exception as WebException;
                    var summary =
                        ExceptionHandling.GetSummaryFromWebException("Update notifications", webException);
                    var errorText = TaskCompletedSummaryStrings.GetDescriptionForResult(summary.Result);
                    this.IsSyncing = false;
                    this.submitErrorInteractionRequest.Raise(
                        new Notification { Title = "Push Notification: Server error", Content = errorText },
                        n => { });
                }
                else
                {
                    throw exception;
                }
            });
        }

        private void CheckCanSubmit()
        {
            this.CanSubmit = !string.IsNullOrEmpty(this.userName) && !string.IsNullOrEmpty(this.password) && !registrationServiceClient.IsProcessing;
        }

        private void CleanUp()
        {
            if (subscription != null) subscription.Dispose();
            registrationServiceClient.CloseChannel();
        }
    }
}
