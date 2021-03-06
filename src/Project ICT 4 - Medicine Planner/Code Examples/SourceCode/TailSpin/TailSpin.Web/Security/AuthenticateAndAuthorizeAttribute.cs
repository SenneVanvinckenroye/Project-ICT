﻿//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Web.Security
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Microsoft.IdentityModel.Protocols.WSFederation;
    using Microsoft.IdentityModel.Web;
    using Samples.Web.ClaimsUtillities;
    using TailSpin.Web.Controllers;
    using TailSpin.Web.Survey.Shared.Models;
    using TailSpin.Web.Survey.Shared.Stores;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class AuthenticateAndAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        public string Roles { get; set; }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsSecureConnection)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentUICulture, "https is required to browse the page: '{0}'.", filterContext.HttpContext.Request.Url.AbsoluteUri));
            }

            var tenantName = (string) filterContext.RouteData.Values["tenant"];
            var tenantController = filterContext.Controller as TenantController;
            var tenant = tenantController.TenantStore.GetTenant(tenantName);
            if (tenant == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture, "'{0}' is not a valid tenant.", tenantName));
            }

            tenantController.Tenant = tenant;

            if ((filterContext.Controller as TenantController) == null)
            {
                throw new NotSupportedException("The AuthenticateAndAuthorize attribute can only be used in controllers that inherit from TenantController.");
            }

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                AuthenticateUser(filterContext);
            }
            else
            {
                this.AuthorizeUser(filterContext);
            }
        }

        private static Uri GetReturnUrl(RequestContext context)
        {
            var request = context.HttpContext.Request;
            var reqUrl = request.Url;
            var wreply = new StringBuilder();

            wreply.Append(reqUrl.Scheme);     // e.g. "http"
            wreply.Append("://");
            wreply.Append(request.Headers["Host"] ?? reqUrl.Authority);
            wreply.Append(request.RawUrl);

            if (!request.ApplicationPath.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                wreply.Append("/");
            }

            return new Uri(wreply.ToString());
        }

        private static string RetrieveHomeRealmForTenant(string tenantName)
        {
            string tenantHomeRealm = ConfigurationManager.AppSettings[tenantName];
            if (string.IsNullOrEmpty(tenantHomeRealm))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture, "No home realm is defined on .config file for tenant '{0}'.", tenantName));
            }

            return tenantHomeRealm;
        }

        private static void AuthenticateUser(AuthorizationContext context)
        {
            var tenantName = (string) context.RouteData.Values["tenant"];

            if (!string.IsNullOrEmpty(tenantName))
            {
                var returnUrl = GetReturnUrl(context.RequestContext);

                // user is not authenticated and it's entering for the first time
                var fam = FederatedAuthentication.WSFederationAuthenticationModule;
                var signIn = new SignInRequestMessage(new Uri(fam.Issuer), fam.Realm)
                {
                    Context = returnUrl.ToString(),
                    HomeRealm = RetrieveHomeRealmForTenant(tenantName)
                };

                // In the Windows Azure environment, build a wreply parameter for  the SignIn request
                // that reflects the real address of the application.
                HttpRequest request = HttpContext.Current.Request;
                Uri requestUrl = request.Url;

                StringBuilder wreply = new StringBuilder();
                wreply.Append(requestUrl.Scheme);     // e.g. "http" or "https"
                wreply.Append("://");
                wreply.Append(request.Headers["Host"] ?? requestUrl.Authority);
                wreply.Append(request.ApplicationPath);

                if (!request.ApplicationPath.EndsWith("/"))
                {
                    wreply.Append("/");
                }

                wreply.Append("FederationResult");

                signIn.Reply = wreply.ToString();

                context.Result = new RedirectResult(signIn.WriteQueryString());
            }
        }

        private void AuthorizeUser(AuthorizationContext context)
        {
            var tenantRequested = (string)context.RouteData.Values["tenant"];
            var userTenant = ClaimHelper.GetCurrentUserClaim(TailSpin.ClaimTypes.Tenant).Value;
            if (!tenantRequested.Equals(userTenant, StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new HttpUnauthorizedResult();
                return;
            }

            var authorizedRoles = this.Roles.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            bool hasValidRole = false;
            foreach (var role in authorizedRoles)
            {
                if (context.HttpContext.User.IsInRole(role.Trim()))
                {
                    hasValidRole = true;
                    break;
                }
            }

            if (!hasValidRole)
            {
                context.Result = new HttpUnauthorizedResult();
                return;
            }
        }
    }
}
