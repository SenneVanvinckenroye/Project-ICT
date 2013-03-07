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
    using System.Web.Mvc;
    using TailSpin.Web.Models;

    public class OnBoardingController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var model = new TenantMasterPageViewData { Title = "On boarding" };
            return View(model);
        }

        [HttpGet]
        public ActionResult Join()
        {
            var model = new TenantMasterPageViewData { Title = "Join Tailspin" };
            return View(model);   
        }
    }
}
