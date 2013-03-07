//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Web.Survey.Shared.SurveysFiltering
{
    using System.Collections.Generic;

    public interface ITenantFilterStore
    {
        void Initialize();
        IEnumerable<string> GetTenants(string username);
        void SetTenants(string username, IEnumerable<string> tenants);
        IEnumerable<string> GetUsers(string tenant);
    }
}