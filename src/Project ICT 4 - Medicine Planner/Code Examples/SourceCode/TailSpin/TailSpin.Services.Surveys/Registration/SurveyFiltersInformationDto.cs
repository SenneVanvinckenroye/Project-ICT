//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Services.Surveys.Registration
{
    public class SurveyFiltersInformationDto
    {
        public SurveyFiltersDto SurveyFilters { get; set; }

        public TenantDto[] AllTenants { get; set; }
    }
}