//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using System.Runtime.Serialization;
using Microsoft.Phone.Shell;
using Microsoft.Practices.Prism.ViewModel;
using TailSpin.PhoneClient.Adapters;

namespace TailSpin.PhoneClient.ViewModels
{
    /// <summary>
    /// This base ViewModel class provides tombstoning support to derived view models. 
    /// The OnPageDeactivation virtual method is called when the user navigates away from a page. 
    /// The OnPageResumeFromTombstoning is called when a page is recovered from tombstoning.
    /// </summary>
    [DataContract]
    public abstract class ViewModel : NotificationObject, IDisposable
    {
        private readonly INavigationService navigationService;
        private readonly IPhoneApplicationServiceFacade phoneApplicationServiceFacade;
        private bool disposed;
        private readonly Uri pageUri;
        private static Uri currentPageUri;

        /// <summary>
        /// The class constructor.
        /// </summary>
        /// <param name="navigationService">This parameter provides the ability to navigate between
        /// pages as well as determine if the application has recovered from tombstoning.</param>
        /// <param name="phoneApplicationServiceFacade">This parameter provides a facade over PhoneApplicationService.Current.State.</param>
        /// <param name="pageUri">This parameter identifies the URI of the page that is associated with this ViewModel instance.</param>
        protected ViewModel(INavigationService navigationService, IPhoneApplicationServiceFacade phoneApplicationServiceFacade, Uri pageUri)
        {
            this.pageUri = pageUri;
            this.navigationService = navigationService;
            this.phoneApplicationServiceFacade = phoneApplicationServiceFacade;

            this.navigationService.Navigated += this.OnNavigationService_Navigated;
            this.navigationService.Navigating += this.OnNavigationService_Navigating;
        }

        /// <summary>
        /// When a navigating event is raised, this event handler identifies the view model of the current page, 
        /// and then calls the OnPageDeactivation method only on the view model of the current page. 
        /// A boolean value is passed into the OnPageDeactivation method so that derived view models can handle two cases:
        /// 1) Navigation was due to page to page navigation (intentional navigation).
        /// 2) Navigation was due to deactivation (unintentional navigation).
        /// </summary>
        void OnNavigationService_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (currentPageUri == null || pageUri == null) return;

            if (currentPageUri.ToString().StartsWith(pageUri.ToString()))
            {
                OnPageDeactivation(e.IsNavigationInitiator);
            }
        }

        /// <summary>
        /// The base ViewModel class uses the navigation service to determine if this view model instance  
        /// needs to recover from tombstoning. If this page needs to recover from tombstoning 
        /// but has not done so, the OnPageResumeFromTombstoning method will be called. 
        /// The navigation service is then notified that this page has completed
        /// tombstone recovery. This ensures that only pages that were tombstoned are resumed via 
        /// the OnPageResumeFromTombstoning method.
        /// </summary>
        void OnNavigationService_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (IsResumingFromTombstoning)
            {
                if (e.Uri.ToString().StartsWith(pageUri.ToString()))
                {
                    OnPageResumeFromTombstoning();
                    navigationService.UpdateTombstonedPageTracking(pageUri);
                }
            }
            currentPageUri = e.Uri;
        }

        ~ViewModel()
        {
            this.Dispose();
        }

        protected bool IsResumingFromTombstoning 
        { 
            get
            {
                return navigationService.DoesPageNeedtoRecoverFromTombstoning(pageUri);
            } 
        }

        public INavigationService NavigationService
        {
            get { return this.navigationService; }
        }

        public IPhoneApplicationServiceFacade PhoneApplicationServiceFacade
        {
            get { return this.phoneApplicationServiceFacade; }
        }

        public virtual void OnPageDeactivation(bool isIntentionalNavigation)
        {
        }

        public abstract void OnPageResumeFromTombstoning();

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                navigationService.Navigated -= this.OnNavigationService_Navigated;
                navigationService.Navigating -= this.OnNavigationService_Navigating;
            }

            this.disposed = true;
        }
    }
}
