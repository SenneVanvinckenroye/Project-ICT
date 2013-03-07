//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;

namespace TailSpin.PhoneServices.Services.Stores
{
    public class SurveyStoreLocator : ISurveyStoreLocator
    {
        private readonly ISettingsStore settingsStore;
        private readonly Func<string, ISurveyStore> surveyStoreFactory;
        private string username;
        private ISurveyStore surveyStore;

        public SurveyStoreLocator(ISettingsStore settingsStore, Func<string, ISurveyStore> surveyStoreFactory)
        {
            this.settingsStore = settingsStore;
            this.surveyStoreFactory = surveyStoreFactory;
        }

        public ISurveyStore GetStore()
        {
            if (string.IsNullOrEmpty(settingsStore.UserName))
            {
                return new NullSurveyStore();
            }

            if (settingsStore.UserName != username)
            {
                username = settingsStore.UserName;
                var storeName = string.Format("{0}.store", username);

                try
                {
                    surveyStore = surveyStoreFactory.Invoke(storeName);
                }
                catch (Exception)
                {
                    return new NullSurveyStore();
                }
            }

            return surveyStore;
        }
    }
}
