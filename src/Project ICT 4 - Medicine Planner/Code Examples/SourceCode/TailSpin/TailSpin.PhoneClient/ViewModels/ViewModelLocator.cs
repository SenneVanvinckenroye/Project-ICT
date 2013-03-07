//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using TailSpin.PhoneClient.Adapters;
using TailSpin.PhoneClient.Services;
using TailSpin.PhoneServices.Services;
using TailSpin.PhoneServices.Services.Stores;

namespace TailSpin.PhoneClient.ViewModels
{
    public class ViewModelLocator : IDisposable
    {
        private readonly ContainerLocator containerLocator;
        private bool disposed;

        public ViewModelLocator()
        {
            this.containerLocator = new ContainerLocator();
        }

        public AppSettingsViewModel AppSettingsViewModel
        {
            get { return this.containerLocator.Container.Resolve<AppSettingsViewModel>(); }
        }

        public FilterSettingsViewModel FilterSettingsViewModel
        {
            get { return this.containerLocator.Container.Resolve<FilterSettingsViewModel>(); }
        }

        public IScheduledActionClient ScheduledActionClient
        {
            get
            {
                return this.containerLocator.Container.Resolve<IScheduledActionClient>();
            }
        }

        public INavigationService NavigationService
        {
            get
            {
                return this.containerLocator.Container.Resolve<INavigationService>();
            }
        }

        public SurveyListViewModel SurveyListViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<SurveyListViewModel>();
            }
        }

        public TakeSurveyViewModel TakeSurveyViewModel
        {
            get
            {
                var vm = new TakeSurveyViewModel(this.containerLocator.Container.Resolve<INavigationService>(),
                                                 this.containerLocator.Container.Resolve<IPhoneApplicationServiceFacade>(),
                                                 this.containerLocator.Container.Resolve<ILocationService>(),
                                                 this.containerLocator.Container.Resolve<ISurveyStoreLocator>(),
                                                 this.containerLocator.Container.Resolve<IShellTile>());

                return vm;
            }
        }

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
                this.containerLocator.Dispose();
            }

            this.disposed = true;
        }
    }
}
