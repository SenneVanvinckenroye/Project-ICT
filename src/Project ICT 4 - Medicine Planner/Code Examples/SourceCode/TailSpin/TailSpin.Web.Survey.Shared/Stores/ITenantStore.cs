//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Web.Survey.Shared.Stores
{
    using System.Collections.Generic;
    using TailSpin.Web.Survey.Shared.Models;

    public interface ITenantStore
    {
        void Initialize();
        Tenant GetTenant(string tenant);
        IEnumerable<Tenant> GetTenants();
        void SaveTenant(Tenant tenant);
        void UploadLogo(string tenant, byte[] logo);
    }
}