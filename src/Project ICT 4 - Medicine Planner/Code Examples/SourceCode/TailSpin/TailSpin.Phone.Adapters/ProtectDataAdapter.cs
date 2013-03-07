//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System.Security.Cryptography;

namespace TailSpin.Phone.Adapters
{
    public class ProtectDataAdapter : IProtectData
    {
        public byte[] Protect(byte[] userData, byte[] optionalEntropy)
        {
            return ProtectedData.Protect(userData, optionalEntropy);
        }

        public byte[] Unprotect(byte[] encryptedData, byte[] optionalEntropy)
        {
            return ProtectedData.Unprotect(encryptedData, optionalEntropy);
        }
    }
}
