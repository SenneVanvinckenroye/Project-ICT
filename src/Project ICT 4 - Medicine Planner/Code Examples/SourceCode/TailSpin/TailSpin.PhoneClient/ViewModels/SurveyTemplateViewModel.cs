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
using System.Runtime.Serialization;
using System.Windows;
using Microsoft.Phone.Shell;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using TailSpin.PhoneClient.Adapters;
using TailSpin.PhoneServices.Models;
using TailSpin.PhoneServices.Services;

namespace TailSpin.PhoneClient.ViewModels
{
    [DataContract]
    public class SurveyTemplateViewModel : NotificationObject
    {
        private int completedAnswers;
        private int completeness;
        private int answersQty;
        private readonly IShellTile shellTile;
        private readonly ILocationService locationService;
        private IPhoneApplicationServiceFacade phoneApplicationServiceFacade;
        private INavigationService navigationService;

        public SurveyTemplateViewModel(
            SurveyTemplate surveyTemplate,
            INavigationService navigationService,
            IPhoneApplicationServiceFacade phoneApplicationServiceFacade,
            IShellTile shellTile,
            ILocationService locationService)
        {
            this.Template = surveyTemplate;
            this.navigationService = navigationService;
            this.phoneApplicationServiceFacade = phoneApplicationServiceFacade;
            this.shellTile = shellTile;
            this.locationService = locationService;

            // The MarkFavoriteCommand simply updates the IsFavorite property of the in-memory copy
            // of the survey template. If you wish this value to survive tombstoning or app closing,
            // this value could be persisted in isolated storage or some other storage mechanism.
            this.MarkFavoriteCommand = new DelegateCommand(() =>
                            {
                                this.Template.IsFavorite = true;
                                this.RaisePropertyChanged(() => this.IsFavorite);
                            });
            this.RemoveFavoriteCommand = new DelegateCommand(() =>
                            {
                                this.Template.IsFavorite = false;
                                this.RaisePropertyChanged(() => this.IsFavorite);
                            });

            this.PinCommand =
                new DelegateCommand(
                    PinToStart,
                    () => IsPinnable);

            this.TakeSurveyCommand = 
                new DelegateCommand(() =>
                        {
                            if (!this.CanTakeSurvey())
                            {
                                MessageBox.Show("Please wait until synchronization completes.");
                                return;
                            }
                            this.phoneApplicationServiceFacade.Save("TakeSurveyId", this.Template.SlugName);
                            this.navigationService.Navigate(BuildUri(@"/Views/TakeSurvey/TakeSurveyView.xaml"));
                        });
            this.CanTakeSurvey = () => true;
        }

        public DelegateCommand TakeSurveyCommand { get; set; }

        public DelegateCommand MarkFavoriteCommand { get; set; }

        public DelegateCommand RemoveFavoriteCommand { get; set; }

        public DelegateCommand PinCommand { get; set; }

        public Func<bool> CanTakeSurvey { get; set; }
            
        [DataMember]
        public SurveyTemplate Template { get; set; }

        public string IconUrl { get { return this.Template.IconUrl; } }

        public bool IsPinnable
        {
            get
            {
                return shellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString() == string.Format(Constants.PinnedSurveyUriFormat, this.Template.SlugName)) == null;
            }
        }

        public string Tenant { get { return this.Template.Tenant; } }

        public string Title { get { return this.Template.Title; } }

        public int Length { get { return this.Template.Length; } }

        public int QuestionsQty { get { return this.Template.Questions.Count; } }

        public int AnswersQty
        {
            get
            {
                return this.answersQty;
            }

            set
            {
                if (this.answersQty != value)
                {
                    this.answersQty = value;
                    this.RaisePropertyChanged(() => this.AnswersQty);
                }
            }
        }

        public bool IsFavorite { get { return this.Template.IsFavorite; } }

        public int CompletedAnswers
        {
            get
            {
                return this.completedAnswers;
            }

            set
            {
                this.completedAnswers = value;
                this.RaisePropertyChanged(() => this.CompletedAnswers);
            }
        }

        public int Completeness
        {
            get
            {
                return this.completeness;
            }

            set
            {
                this.completeness = value;
                this.RaisePropertyChanged(() => this.Completeness);
            }
        }

        private static Uri BuildUri(string baseUriString)
        {
            return new Uri(baseUriString, UriKind.Relative);
        }

        public void PinToStart()
        {
            var tile = shellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString() == string.Format(Constants.PinnedSurveyUriFormat, this.Template.SlugName));
            if (tile == null)
            {
                var tileData = new StandardTileData
                {
                    Title = this.Template.Title,
                    BackgroundImage = new Uri("/Background.png", UriKind.Relative)
                };

                shellTile.Create(new Uri(string.Format(Constants.PinnedSurveyUriFormat, this.Template.SlugName), UriKind.Relative), tileData);
            }       
        }
    }
}
