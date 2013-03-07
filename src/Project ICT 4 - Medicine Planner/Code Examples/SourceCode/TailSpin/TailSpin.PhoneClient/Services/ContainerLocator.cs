//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System.ComponentModel;
using System.Device.Location;
using TailSpin.Phone.Adapters;
using TailSpin.PhoneClient.Adapters;
using TailSpin.PhoneClient.Services.RegistrationService;
using TailSpin.PhoneServices.Services;
using TailSpin.PhoneServices.Services.Clients;
using TailSpin.PhoneServices.Services.Stores;
using TailSpin.PhoneServices.Services.SurveysService;
using System;
using System.Windows;
using Funq;
using TailSpin.PhoneClient.ViewModels;

namespace TailSpin.PhoneClient.Services
{
    public class ContainerLocator : IDisposable
    {
        private bool disposed;

        public ContainerLocator()
        {
            this.Container = new Container();
            this.ConfigureContainer();
        }

        public Container Container { get; private set; }

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
                this.Container.Dispose();
            }

            this.disposed = true;
        }

        private void ConfigureContainer()
        {
            this.Container.Register("ServiceUri", new Uri("http://127.0.0.1:8080/Survey/"));
            this.Container.Register("RegistrationServiceUri", new Uri("http://127.0.0.1:8080/Registration/"));

            if (DesignerProperties.IsInDesignTool)
            {
                this.Container.Register<ISettingsStore>(c => new DesignerSettingsStore());
            }
            else
            {
                this.Container.Register<ISettingsStore>(c => new SettingsStore(c.Resolve<IProtectData>()));
            }

            this.Container.Register<IProtectData>(c => new ProtectDataAdapter());
            this.Container.Register<IHttpClient>(c => new HttpClient());
            this.Container.Register<IPhoneApplicationServiceFacade>(c => new PhoneApplicationServiceFacade());
            this.Container.Register<INavigationService>(_ =>
                new ApplicationFrameNavigationService(((App)Application.Current).RootFrame));

            this.Container.Register<IScheduledActionService>(c => new ScheduledActionServiceAdapter());
            this.Container.Register<IScheduledActionClient>(c => new ScheduledActionClient(c.Resolve<ISettingsStore>(), c.Resolve<IScheduledActionService>()));
            this.Container.Register<IShellTile>(c => new ShellTileAdapter());
            this.Container.Register<IGeoCoordinateWatcher>(c => new GeoCoordinateWatcherAdapter(GeoPositionAccuracy.Default));

            this.Container.Register<ILocationService>(c => new LocationService(c.Resolve<ISettingsStore>(), c.Resolve<IGeoCoordinateWatcher>()));
            this.Container.Register<IMessageBox>(c => new MessageBoxAdapter());

            // View Models
            this.Container.Register(
                c => new AppSettingsViewModel(
                         c.Resolve<ISettingsStore>(),
                         c.Resolve<IRegistrationServiceClient>(),
                         c.Resolve<INavigationService>(),
                         c.Resolve<IPhoneApplicationServiceFacade>(),
                         c.Resolve<IScheduledActionClient>(),
                         c.Resolve<IMessageBox>()))
                .ReusedWithin(ReuseScope.None);

            // The SurveyStore uses the Isolated Storage file system to store both survey questions
            // and survey answers. One file is used to store questions and another for answers.
            // If your scenario requires simultaneous updating questions or answers from different 
            // places in the application, then using a database would appropriate.

            this.Container.Register<ISurveyStoreLocator>(
                c => new SurveyStoreLocator(
                         c.Resolve<ISettingsStore>(),
                         storeName => new SurveyStore(storeName)));

            this.Container.Register(
                c => new FilterSettingsViewModel(
                         c.Resolve<IRegistrationServiceClient>(),
                         c.Resolve<INavigationService>(),
                         c.Resolve<IPhoneApplicationServiceFacade>(),
                         c.Resolve<ISurveyStoreLocator>(),
                         c.Resolve<IMessageBox>()))
                .ReusedWithin(ReuseScope.None);

            this.Container.Register<ISurveysSynchronizationService>(
                c => new SurveysSynchronizationService(
                         c.Resolve<ISurveysServiceClient>,
                         c.Resolve<ISurveyStoreLocator>()));
            this.Container.Register(
                c => new SurveyListViewModel(
                         c.Resolve<ISurveyStoreLocator>(),
                         c.Resolve<ISurveysSynchronizationService>(),
                         c.Resolve<INavigationService>(),
                         c.Resolve<IPhoneApplicationServiceFacade>(),
                         c.Resolve<IShellTile>(),
                         c.Resolve<ISettingsStore>(),
                         c.Resolve<ILocationService>()));

            //// The ONLY_PHONE symbol is only defined in the "OnlyPhone" configuration to run the phone project standalone
#if ONLY_PHONE
            this.Container.Register<ISurveysServiceClient>(c => new SurveysServiceClientMock(c.Resolve<ISettingsStore>()));
            this.Container.Register<IRegistrationServiceClient>(c => new RegistrationServiceClientMock(c.Resolve<ISettingsStore>()));
#else
            this.Container.Register<ISurveysServiceClient>(c => new SurveysServiceClient(c.ResolveNamed<Uri>("ServiceUri"), c.Resolve<ISettingsStore>(), c.Resolve<IHttpClient>()));
            this.Container.Register<IRegistrationServiceClient>(c => new RegistrationServiceClient(c.ResolveNamed<Uri>("RegistrationServiceUri"), c.Resolve<ISettingsStore>(), c.Resolve<IHttpClient>()));
#endif
        }
    }
}
