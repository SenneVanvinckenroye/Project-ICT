//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using System.IO;
using TailSpin.Phone.Adapters;

namespace TailSpin.Phone.TestSupport
{
    public class MockIsolatedStorageFacade : IIsolatedStorageFacade
    {
        public Func<string, BinaryWriter> OpenBinaryWriterTestCallback { get; set; }

        public BinaryWriter OpenBinaryWriter(string outputFileName)
        {
            if (OpenBinaryWriterTestCallback != null)
            {
                OpenBinaryWriterTestCallback(outputFileName);
            }
            return new BinaryWriter(Stream.Null);
        }

        public void Dispose()
        {
            
        }
    }
}
