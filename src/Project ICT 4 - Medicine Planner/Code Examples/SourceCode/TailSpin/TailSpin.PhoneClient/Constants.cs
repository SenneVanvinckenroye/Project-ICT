//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.PhoneClient
{
    public static class Constants
    {
        public const string PeriodicTaskName = "NewSurveysTask";
        public const string PeriodicTaskDescription = "This task checks for new surveys.";
        public const string PinnedSurveyUriFormat = "/Views/TakeSurvey/TakeSurveyView.xaml?surveyID={0}";
        public const string ResourceTaskName = "UploadSurveysTask";
        public const string ResourceTaskDescription = "This task uploads completed surveys.";
        public const string DisabledBackgroundException = "BNS Error: The action is disabled";
        public const string DisabledBackgroundCaption = "Unable to Schedule Tasks";
        public const string DisabledBackgroundMessage = "Background tasks for this application have been disabled and can be re-enabled by navigating to Start > settings > applications > background tasks";
    }
}
