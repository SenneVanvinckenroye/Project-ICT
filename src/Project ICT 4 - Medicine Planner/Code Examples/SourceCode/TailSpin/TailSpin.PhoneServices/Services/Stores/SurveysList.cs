//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System.Collections.Generic;
using TailSpin.PhoneServices.Models;

namespace TailSpin.PhoneServices.Services.Stores
{
    public class SurveysList
    {
        public SurveysList()
        {
            this.LastSyncDate = string.Empty;
        }

        public List<SurveyTemplate> Templates { get; set; }

        public List<SurveyAnswer> Answers { get; set; }

        public string LastSyncDate { get; set; }

        public int UnopenedCount { get; set; }
    }
}
