//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using TailSpin.Phone.Adapters;

namespace TailSpin.Phone.TestSupport
{
    public class MockProtectDataAdapter : IProtectData
    {
        public Func<byte[], byte[], byte[]> ProtectTestCallback { get; set; }

        public Func<byte[], byte[], byte[]> UnProtectTestCallback { get; set; }

        public byte[] Protect(byte[] userData, byte[] optionalEntropy)
        {
            if(ProtectTestCallback == null)
            {
                throw new InvalidOperationException("Must set ProtectTestCallback.");
            }
            return ProtectTestCallback(userData, optionalEntropy);
        }

        public byte[] Unprotect(byte[] encryptedData, byte[] optionalEntropy)
        {
            if (UnProtectTestCallback == null)
            {
                throw new InvalidOperationException("Must set UnProtectTestCallback.");
            }
            return UnProtectTestCallback(encryptedData, optionalEntropy);
        }
    }
}
