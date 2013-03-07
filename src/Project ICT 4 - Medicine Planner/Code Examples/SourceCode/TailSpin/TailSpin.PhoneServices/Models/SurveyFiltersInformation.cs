//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System.Collections.Generic;

namespace TailSpin.PhoneServices.Models
{
    public class SurveyFiltersInformation
    {
        public SurveyFiltersInformation()
        {
            AllTenants = new TenantItem[0];
            SelectedTenants = new TenantItem[0];
        }

        public IEnumerable<TenantItem> AllTenants { get; set; }
        public IEnumerable<TenantItem> SelectedTenants { get; set; }
    }
}
