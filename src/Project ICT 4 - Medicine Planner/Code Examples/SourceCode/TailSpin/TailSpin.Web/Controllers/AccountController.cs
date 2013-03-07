//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Web.Controllers
{
    using System;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using TailSpin.Web.Security;
    using TailSpin.Web.Survey.Shared.Stores;

    [RequireHttps]
    [AuthenticateAndAuthorize(Roles = "Survey Administrator")]
    public class AccountController : TenantController 
    {
        public AccountController(ITenantStore tenantStore) : base(tenantStore)
        {
        }

        public ActionResult Index()
        {
            var model = this.CreateTenantPageViewData(this.Tenant);
            model.Title = "My Account";
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadLogo(string tenant, HttpPostedFileBase newLogo)
        {
            if (newLogo != null && newLogo.ContentLength > 0)
            {
                this.TenantStore.UploadLogo(tenant, new BinaryReader(newLogo.InputStream).ReadBytes(Convert.ToInt32(newLogo.InputStream.Length)));
            }

            return this.RedirectToAction("Index");
        }
    }
}