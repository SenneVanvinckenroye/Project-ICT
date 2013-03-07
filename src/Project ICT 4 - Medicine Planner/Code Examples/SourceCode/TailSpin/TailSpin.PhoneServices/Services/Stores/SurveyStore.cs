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
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using TailSpin.PhoneServices.Models;

namespace TailSpin.PhoneServices.Services.Stores
{
    public class SurveyStore : ISurveyStore
    {
        private readonly IsolatedStorageSettings isolatedStore;
        private const string lastSyncDateKey = "lastSyncDateKey";
        private const string unopenedCountKey = "unopenedCountKey";
        private readonly string storeName;

        public SurveyStore(string storeName)
        {
            this.storeName = storeName;
            isolatedStore = IsolatedStorageSettings.ApplicationSettings;
            Initialize();
        }

        public SurveysList AllSurveys { get; set; }

        public string LastSyncDate
        {
            get { return AllSurveys.LastSyncDate; }

            set
            {
                AllSurveys.LastSyncDate = value;
                SaveLastSyncDate();
            }
        }

        public IEnumerable<SurveyTemplate> GetSurveyTemplates()
        {
            return AllSurveys.Templates;
        }

        public IEnumerable<SurveyAnswer> GetCompleteSurveyAnswers()
        {
            return AllSurveys.Answers.Where(a => a.IsComplete);
        }

        public void MarkSurveyTemplateRead(SurveyTemplate template)
        {
            var survey = AllSurveys.Templates.Single(s => s.SlugName == template.SlugName && s.Tenant == template.Tenant);
            if (survey != null)
            {  
                survey.IsNew = false;
                SaveTemplates();
            }       
        }

        public event EventHandler SurveyAnswerSaved;

        public void SaveSurveyTemplates(IEnumerable<SurveyTemplate> surveys)
        {
            foreach (var s in surveys)
            {
                s.IsNew = true;
            }

            var newSurveys = surveys.Where(
                ns => !AllSurveys.Templates.Any(s => s.SlugName == ns.SlugName && s.Tenant == ns.Tenant));

            AllSurveys.UnopenedCount += newSurveys.Count();
            SaveUnopenedCount();

            AllSurveys.Templates.AddRange(newSurveys);
            SaveTemplates();
        }

        public void SaveSurveyAnswer(SurveyAnswer surveyAnswer)
        {
            if (!AllSurveys.Answers.Contains(surveyAnswer))
            {
                AllSurveys.Answers.Add(surveyAnswer);
            }

            SaveAnswers();
            
            var handler = this.SurveyAnswerSaved;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        private void SaveLastSyncDate()
        {
            if (isolatedStore.Contains(lastSyncDateKey))
            {
                isolatedStore[lastSyncDateKey] = AllSurveys.LastSyncDate;
            }
            else
            {
                isolatedStore.Add(lastSyncDateKey, AllSurveys.LastSyncDate);
            }
            isolatedStore.Save();
        }

        public void SaveUnopenedCount()
        {
            if (isolatedStore.Contains(unopenedCountKey))
            {
                isolatedStore[unopenedCountKey] = AllSurveys.UnopenedCount;
            }
            else
            {
                isolatedStore.Add(unopenedCountKey, AllSurveys.UnopenedCount);
            }
            isolatedStore.Save();
        }

        private void SaveTemplates()
        {
            using (var filesystem = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var fs = new IsolatedStorageFileStream(storeName + ".templates", FileMode.Create, filesystem))
                {
                    var serializer =
                        new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(List<SurveyTemplate>));
                    serializer.WriteObject(fs, AllSurveys.Templates);
                }
            }
        }

        private void SaveAnswers()
        {
            using (var filesystem = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var fs = new IsolatedStorageFileStream(storeName + ".answers", FileMode.Create, filesystem))
                {
                    var serializer =
                        new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(List<SurveyAnswer>));
                    serializer.WriteObject(fs, AllSurveys.Answers);
                }
            }
        }

        public void SaveStore()
        {
            SaveLastSyncDate();
            SaveTemplates();
            SaveAnswers();
        }

        public SurveyAnswer GetCurrentAnswerForTemplate(SurveyTemplate template)
        {
            return
                AllSurveys.Answers.Where(
                    a => template.SlugName == a.SlugName && template.Tenant == a.Tenant && !a.IsComplete).FirstOrDefault
                    ();
        }

        /// <summary>
        /// This method deletes files related to completed voice and picture questions.
        /// You may want to also delete files that were orphaned when the user creates voice recordings
        /// or picture files, causes this app to go dormant, and then relaunches the app.
        /// </summary>
        /// <param name="surveyAnswersToDelete"></param>
        public void DeleteSurveyAnswers(IEnumerable<SurveyAnswer> surveyAnswersToDelete)
        {
            var filesToDelete =
                from answers in surveyAnswersToDelete
                from answer in answers.Answers
                where
                    answer.Value != null &&
                    (answer.QuestionType == QuestionType.Voice || answer.QuestionType == QuestionType.Picture)
                select answer.Value;

            using (var filesystem = IsolatedStorageFile.GetUserStoreForApplication())
            {
                foreach (var file in filesToDelete)
                {
                    if (filesystem.FileExists(file))
                    {
                        filesystem.DeleteFile(file);
                    }
                }
            }

            surveyAnswersToDelete.ToList().ForEach(a => AllSurveys.Answers.Remove(a));

            SaveAnswers();
        }

        private void Initialize()
        {
            AllSurveys = new SurveysList();

            if (isolatedStore.Contains(lastSyncDateKey))
            {
                AllSurveys.LastSyncDate = (string)isolatedStore[lastSyncDateKey];
            }

            if (isolatedStore.Contains(unopenedCountKey))
            {
                AllSurveys.UnopenedCount = (int)isolatedStore[unopenedCountKey];
            }

            using (var filesystem = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!filesystem.FileExists(storeName + ".templates"))
                {
                    AllSurveys.Templates = new List<SurveyTemplate>();
                }
                else
                {
                    using (var fs = new IsolatedStorageFileStream(storeName + ".templates", FileMode.Open, filesystem))
                    {
                        var serializer =
                            new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(List<SurveyTemplate>));
                        AllSurveys.Templates = serializer.ReadObject(fs) as List<SurveyTemplate>;
                    }
                }

                if (!filesystem.FileExists(storeName + ".answers"))
                {
                    AllSurveys.Answers = new List<SurveyAnswer>();
                }
                else
                {
                    using (var fs = new IsolatedStorageFileStream(storeName + ".answers", FileMode.Open, filesystem))
                    {
                        var serializer =
                            new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(List<SurveyAnswer>));
                        AllSurveys.Answers = serializer.ReadObject(fs) as List<SurveyAnswer>;
                    }
                }
            }
        }
    }
}
