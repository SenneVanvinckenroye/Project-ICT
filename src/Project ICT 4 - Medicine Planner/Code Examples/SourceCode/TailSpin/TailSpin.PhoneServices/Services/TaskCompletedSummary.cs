//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.PhoneServices.Services
{
    public class TaskCompletedSummary
    {
        public string Task { get; set; }

        public TaskSummaryResult Result { get; set; }

        public string Context { get; set; }
    }
}
