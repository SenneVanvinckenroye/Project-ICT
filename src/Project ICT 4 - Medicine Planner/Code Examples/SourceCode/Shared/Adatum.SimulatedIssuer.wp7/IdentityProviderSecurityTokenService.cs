﻿//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace Adatum.SimulatedIssuer
{
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;
    using System.Web.Configuration;
    using Microsoft.IdentityModel.Claims;
    using Microsoft.IdentityModel.Configuration;
    using Microsoft.IdentityModel.Protocols.WSTrust;
    using Microsoft.IdentityModel.SecurityTokenService;
    using Samples.Web.ClaimsUtillities;

    public class IdentityProviderSecurityTokenService : SecurityTokenService
    {
        public IdentityProviderSecurityTokenService(SecurityTokenServiceConfiguration configuration)
            : base(configuration)
        {
        }

        protected override Scope GetScope(IClaimsPrincipal principal, RequestSecurityToken request)
        {
            Scope scope = new Scope(request.AppliesTo.Uri.AbsoluteUri, SecurityTokenServiceConfiguration.SigningCredentials);

            string encryptingCertificateName = WebConfigurationManager.AppSettings[ApplicationSettingsNames.EncryptingCertificateName];
            if (!string.IsNullOrEmpty(encryptingCertificateName))
            {
                scope.EncryptingCredentials = new X509EncryptingCredentials(CertificateUtilities.GetCertificate(StoreName.My, StoreLocation.LocalMachine, encryptingCertificateName));
            }
            else
            {
                scope.TokenEncryptionRequired = false;
            }

            if (!string.IsNullOrEmpty(request.ReplyTo))
            {
                scope.ReplyToAddress = request.ReplyTo;
            }
            else
            {
                scope.ReplyToAddress = scope.AppliesToAddress;
            }
                        
            return scope;
        }

        protected override IClaimsIdentity GetOutputClaimsIdentity(IClaimsPrincipal principal, RequestSecurityToken request, Scope scope)
        {
            var outputIdentity = new ClaimsIdentity();

            if (null == principal)
            {
                throw new InvalidRequestException("The caller's principal is null.");
            }

            switch (principal.Identity.Name.ToUpperInvariant())
            {
                // In a production environment, all the information that will be added
                // as claims should be read from the authenticated Windows Principal.
                // The following lines are hardcoded because windows integrated 
                // authentication is disabled.
                case "ADATUM\\JOHNDOE":
                    outputIdentity.Claims.AddRange(new List<Claim>
                       {
                           new Claim(System.IdentityModel.Claims.ClaimTypes.Name, "ADATUM\\johndoe"), 
                           new Claim(AllOrganizations.ClaimTypes.Group, Adatum.Groups.DomainUsers),
                           new Claim(AllOrganizations.ClaimTypes.Group, Adatum.Groups.MarketingManagers)
                       });
                    break;
            }

            return outputIdentity;
        }
    }
}
