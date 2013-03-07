//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using TailSpin.PhoneClient.Adapters;
using TailSpin.PhoneClient.Services.RegistrationService;
using TailSpin.PhoneServices.Models;
using TailSpin.PhoneServices.Services;
using TailSpin.PhoneServices.Services.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Reactive;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Notification = Microsoft.Practices.Prism.Interactivity.InteractionRequest.Notification;
using System.Windows;

namespace TailSpin.PhoneClient.ViewModels
{
    public class FilterSettingsViewModel : ViewModel
    {
        private readonly IRegistrationServiceClient registrationServiceClient;
        private readonly InteractionRequest<Notification> submitErrorInteractionRequest;
        private readonly ISurveyStoreLocator surveyStoreLocator;
        private readonly IMessageBox messageBox;
        private readonly ObservableCollection<TenantItem> tenants;
        private List<TenantItem> tombstonedTenants;
        private bool canSubmit;
        private bool isSyncing;
        private IEnumerable<TenantItem> selectedTenantsInService;

        public FilterSettingsViewModel(
            IRegistrationServiceClient registrationServiceClient, 
            INavigationService navigationService,
            IPhoneApplicationServiceFacade phoneApplicationServiceFacade,
            ISurveyStoreLocator surveyStoreLocator,
            IMessageBox messageBox)
            : base(navigationService, phoneApplicationServiceFacade, new Uri("/Views/FilterSettingsView.xaml", UriKind.Relative))
        {
            this.registrationServiceClient = registrationServiceClient;
            this.surveyStoreLocator = surveyStoreLocator;
            this.messageBox = messageBox;
            this.submitErrorInteractionRequest = new InteractionRequest<Notification>();
            this.tenants = new ObservableCollection<TenantItem>();

            this.SaveCommand = new DelegateCommand(this.Submit, () => this.CanSubmit);
            this.CancelCommand = new DelegateCommand(this.Cancel, () => true);

            if (this.IsResumingFromTombstoning) return;

            if (NetworkAvailable)
            {
                this.CanSubmit = false;
                this.IsSyncing = true;

                this.registrationServiceClient
                    .GetSurveysFilterInformation()
                    .ObserveOnDispatcher()
                    .Subscribe(
                        surveyFiltersInformation =>
                            {
                                this.selectedTenantsInService = surveyFiltersInformation.SelectedTenants;
                                surveyFiltersInformation.AllTenants.ToList().ForEach(t => this.tenants.Add(t));

                                if (surveyFiltersInformation.SelectedTenants.Count() > 0)
                                {
                                    this.tenants
                                        .Intersect(surveyFiltersInformation.SelectedTenants, new TenantItemComparer())
                                        .ToList().ForEach(t => t.IncludeInFilter = true);
                                }

                                this.CanSubmit = true;
                                this.IsSyncing = false;
                            },
                        exception =>
                            {
                                if (exception is WebException)
                                {
                                    this.HandleWebException(exception as WebException, () =>
                                                        {
                                                            if (this.NavigationService.CanGoBack)
                                                                this.NavigationService.GoBack();
                                                        });
                                }
                                else if (exception is UnauthorizedAccessException)
                                {
                                    this.HandleUnauthorizedException(() =>
                                                        {
                                                            if (this.NavigationService.CanGoBack) 
                                                                this.NavigationService.GoBack();
                                                        });
                                }
                                else
                                {
                                    throw exception;
                                }
                            });
            }
            else
            {
                messageBox.Show("Filter preference unable to be retrieved", "Network Unavailable", MessageBoxButton.OK);
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
                    this.SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public IInteractionRequest SubmitErrorInteractionRequest { get { return this.submitErrorInteractionRequest; } }

        public bool IsSyncing
        {
            get { return this.isSyncing; }
            set
            {
                this.isSyncing = value;
                this.RaisePropertyChanged(() => this.IsSyncing);
            }
        }

        public bool NetworkAvailable
        {
            get { return NetworkInterface.NetworkInterfaceType != NetworkInterfaceType.None; }
        }

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public ObservableCollection<TenantItem> Tenants
        {
            get { return this.tenants; }
        }

        public void Submit()
        {
            if (NetworkAvailable)
            {
                this.IsSyncing = true;

                var selectedTenants = (from tenant in this.tenants where tenant.IncludeInFilter select tenant).ToList();

                if (!this.selectedTenantsInService.SequenceEqual(selectedTenants, new TenantItemComparer()))
                {
                    this.registrationServiceClient
                        .UpdateTenants(selectedTenants)
                        .ObserveOnDispatcher()
                        .Subscribe(
                            unused =>
                            {
                                this.selectedTenantsInService = selectedTenants;
                                this.surveyStoreLocator.GetStore().LastSyncDate = string.Empty;
                                this.IsSyncing = false;
                                if (this.NavigationService.CanGoBack) this.NavigationService.GoBack();
                            },
                            exception =>
                            {
                                if (exception is WebException)
                                {
                                    HandleWebException(exception as WebException, () => { });
                                }
                                else if (exception is InvalidOperationException)
                                {
                                    this.IsSyncing = false;
                                    this.submitErrorInteractionRequest.Raise(
                                        new Notification { Title = "Warning", Content = exception.Message },
                                        n => { });
                                }
                                else
                                {
                                    throw exception;
                                }
                            });
                }
                else
                {
                    this.IsSyncing = false;
                    if (this.NavigationService.CanGoBack) this.NavigationService.GoBack();
                }
            }
            else
            {
                messageBox.Show("Filter preference unable to be updated", "Network Unavailable", MessageBoxButton.OK);
            }
        }

        public void Cancel()
        {
            if (this.NavigationService.CanGoBack) this.NavigationService.GoBack();
        }

        public override void OnPageDeactivation(bool isIntentionalNavigation)
        {
            base.OnPageDeactivation(isIntentionalNavigation);

            if (isIntentionalNavigation)
            {
                this.Dispose();
                return;
            }

            if (this.Tenants != null)
            {
                PhoneApplicationServiceFacade.Save("SettingsTenants", this.Tenants.ToList());
            }

            if (this.selectedTenantsInService != null)
            {
                PhoneApplicationServiceFacade.Save("SettingsSelectedTenantsInService", this.selectedTenantsInService.ToList());
            }
        }

        public override sealed void OnPageResumeFromTombstoning()
        {
            this.tombstonedTenants = PhoneApplicationServiceFacade.Load<List<TenantItem>>("SettingsTenants");
            this.selectedTenantsInService = PhoneApplicationServiceFacade.Load<List<TenantItem>>("SettingsSelectedTenantsInService");

            this.Tenants.Clear();
            if (this.tombstonedTenants != null)
            {
                this.tombstonedTenants.ForEach(t => this.Tenants.Add(t));                
            }

            this.CanSubmit = true;
        }

        private void HandleWebException(WebException webException, Action afterNotification)
        {
            var summary = ExceptionHandling.GetSummaryFromWebException(string.Empty, webException);
            var errorText = TaskCompletedSummaryStrings.GetDescriptionForResult(summary.Result);
            this.IsSyncing = false;
            this.submitErrorInteractionRequest.Raise(
                new Notification { Title = "Server error", Content = errorText },
                n => afterNotification());
        }

        private void HandleUnauthorizedException(Action afterNotification)
        {
            this.IsSyncing = false;
            this.submitErrorInteractionRequest.Raise(
                new Notification 
                { 
                    Title = "Server error", 
                    Content = TaskCompletedSummaryStrings.GetDescriptionForResult(TaskSummaryResult.AccessDenied) 
                },
                n => afterNotification());
        }

        private class TenantItemComparer : IEqualityComparer<TenantItem>
        {
            public bool Equals(TenantItem x, TenantItem y)
            {
                return x.Key == y.Key;
            }

            public int GetHashCode(TenantItem obj)
            {
                return obj.Key.GetHashCode();
            }
        }
    }
}
