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

namespace TailSpin.Phone.Adapters
{
    public interface IIsolatedStorageFacade : IDisposable
    {
        BinaryWriter OpenBinaryWriter(string outputFileName);
    }
}
