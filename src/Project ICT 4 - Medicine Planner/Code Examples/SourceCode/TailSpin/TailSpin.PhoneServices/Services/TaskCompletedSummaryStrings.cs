//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using System.Globalization;

namespace TailSpin.PhoneServices.Services
{
    public static class TaskCompletedSummaryStrings
    {
        public static string GetDescriptionForSummary(TaskCompletedSummary summary)
        {
            switch (summary.Result)
            {
                case TaskSummaryResult.Success:
                    var suffix = "s";
                    switch (summary.Task)
                    {
                        case SurveysSynchronizationService.GetSurveysTask:
                            if (summary.Context == "1")
                                suffix = "";

                            return string.Format(
                                CultureInfo.InvariantCulture,
                                "{0} new survey{1}",
                                summary.Context, suffix);
                        case SurveysSynchronizationService.SaveSurveyAnswersTask:
                            if (summary.Context == "1")
                                suffix = "";

                            return string.Format(
                                CultureInfo.InvariantCulture,
                                "{0} answer{1} sent",
                                summary.Context, suffix);
                        default:
                            return string.Format(
                                CultureInfo.InvariantCulture,
                                "{0}: {1}",
                                summary.Task,
                                TaskCompletedSummaryStrings.GetDescriptionForResult(summary.Result));
                    }
                default:
                    return TaskCompletedSummaryStrings.GetDescriptionForResult(summary.Result);
            }
        }

        public static string GetDescriptionForResult(TaskSummaryResult result)
        {
            switch (result)
            {
                case TaskSummaryResult.Success:
                    return "Completed successfully";
                case TaskSummaryResult.AccessDenied:
                    return "The credentials you have configured were invalid. Please enter a valid username and password in the Settings page and try again.";
                case TaskSummaryResult.UnreachableServer:
                    return "The connection to our server couldn't be established. This would be caused by a temporary network issue. Please try again later.";
                case TaskSummaryResult.UnknownError:
                    return "There was an unknown error. Please try again later.";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
