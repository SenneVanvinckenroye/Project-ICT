//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


//===============================================================================
// Microsoft patterns & practices
// Windows Azure Architecture Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wag.codeplex.com/license)
//===============================================================================


[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("BEST.Security.Config", "ASPCONFIG:SecureCookie", Justification = "The Anti-forgery token requires a cookie and this site is not SSL.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.WebConfigurationSecurity", "CA3119:HttpCookiesRequireSslRule", Justification = "The Anti-forgery token requires a cookie and this site is not SSL.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.WebConfigurationSecurity", "CA3128:MachineKeyValidationKeyRule", Justification = "Need this to support Azure. Need the same key for each machine to support validation token.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.WebConfigurationSecurity", "CA3126:MachineKeyDecryptionKeyRule", Justification = "Need this to support Azure. Need the same key for each machine to support validation token.")]