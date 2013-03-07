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
using TailSpin.PhoneServices.Models;
using TailSpin.PhoneServices.Services.Stores;

namespace TailSpin.PhoneClient.Tests.ViewModels.Mocks
{
    public class SurveyStoreMock : ISurveyStore
    {
        //private List<SurveyTemplate> surveys;

        public SurveyStoreMock()
        {
            //this.surveys = new List<SurveyTemplate>();
            this.SurveyAnswers = new List<SurveyAnswer>();
            this.AllSurveys = new SurveysList()
                                  {
                                      LastSyncDate = "",
                                      Templates = new List<SurveyTemplate>(),
                                      Answers = new List<SurveyAnswer>()
                                  };
        }

        public SurveyStoreMock(List<SurveyTemplate> surveys)
        {
            this.AllSurveys = new SurveysList()
            {
                LastSyncDate = "",
                Answers = new List<SurveyAnswer>()
            };
            AllSurveys.Templates = surveys;
            //this.surveys = surveys;
        }

        public IEnumerable<SurveyTemplate> SavedSurveys { get; private set; }

        public IEnumerable<SurveyAnswer> SurveyAnswers { get; set; }

        public string LastSyncDate { get; set; }

        public SurveysList AllSurveys { get; set; }

        public SurveyAnswer GetCurrentAnswerForTemplate(SurveyTemplate template)
        {
            return null;
        }

        public void DeleteSurveyAnswers(IEnumerable<SurveyAnswer> surveyAnswers)
        {
        }

        public IEnumerable<SurveyAnswer> GetCompleteSurveyAnswers()
        {
            return this.SurveyAnswers;
        }

        public IEnumerable<SurveyTemplate> GetSurveyTemplates()
        {
            return AllSurveys.Templates;
            //return this.surveys;
        }

        public void Initialize()
        {
            //if (this.surveys == null)
            //{
            //    this.surveys = new List<SurveyTemplate>(
            //        new[]
            //            {
            //                new SurveyTemplate
            //                    {
            //                        Title = "My survey1",
            //                        Tenant = "Tenant One",
            //                        IconUrl = "http://icon",
            //                        Completeness = 100,
            //                        IsFavorite = true,
            //                        IsNew = true,
            //                        Length = 40
            //                    },
            //                new SurveyTemplate
            //                    {
            //                        Title = "My survey2",
            //                        Tenant = "Tenant One",
            //                        IconUrl = "http://icon",
            //                        Completeness = 50,
            //                        IsFavorite = true,
            //                        IsNew = false,
            //                        Length = 40
            //                    },
            //                new SurveyTemplate
            //                    {
            //                        Title = "My survey3",
            //                        Tenant = "Tenant One",
            //                        IconUrl = "http://icon",
            //                        Completeness = 100,
            //                        IsFavorite = false,
            //                        IsNew = false,
            //                        Length = 40
            //                    },
            //                new SurveyTemplate
            //                    {
            //                        Title = "My survey4",
            //                        Tenant = "Tenant Two",
            //                        IconUrl = "http://icon",
            //                        Completeness = 50,
            //                        IsFavorite = false,
            //                        IsNew = true,
            //                        Length = 40
            //                    },
            //                new SurveyTemplate
            //                    {
            //                        Title = "My survey5",
            //                        Tenant = "Tenant Two",
            //                        IconUrl = "http://icon",
            //                        Completeness = 100,
            //                        IsFavorite = false,
            //                        IsNew = false,
            //                        Length = 40
            //                    },
            //                new SurveyTemplate
            //                    {
            //                        Title = "My survey6",
            //                        Tenant = "Tenant Two",
            //                        IconUrl = "http://icon",
            //                        Completeness = 100,
            //                        IsFavorite = true,
            //                        IsNew = false,
            //                        Length = 40
            //                    },
            //            });
            //}
        }

        public void SaveSurveyAnswer(SurveyAnswer answer)
        {
        }

        public void SaveSurveyTemplates(IEnumerable<SurveyTemplate> surveys)
        {
            this.SavedSurveys = surveys;
        }

        public void SaveStore()
        {
        }

        public void SaveUnopenedCount()
        {
        }

        public void MarkSurveyTemplateRead(SurveyTemplate template)
        {
        }

        public event EventHandler SurveyAnswerSaved;
    }
}
