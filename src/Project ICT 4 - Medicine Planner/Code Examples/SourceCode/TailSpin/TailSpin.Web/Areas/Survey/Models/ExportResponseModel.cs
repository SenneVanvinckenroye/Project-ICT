//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Web.Areas.Survey.Models
{
    using Web.Survey.Shared.Models;

    public class ExportResponseModel
    {
        public bool HasResponses { get; set; }

        public Tenant Tenant { get; set; }
    }
}