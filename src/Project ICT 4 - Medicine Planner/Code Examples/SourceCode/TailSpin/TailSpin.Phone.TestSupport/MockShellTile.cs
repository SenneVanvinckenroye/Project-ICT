//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


using System;
using System.Collections.Generic;
using Microsoft.Phone.Shell;
using TailSpin.PhoneClient.Adapters;

namespace TailSpin.Phone.TestSupport
{
    public class MockShellTile : IShellTile
    {
        public MockShellTile()
        {
            ActiveTiles = new List<ShellTile>();
        }

        public IEnumerable<ShellTile> ActiveTiles { get; set; }

        public Uri NavigationUri { get; set; }

        public void Create(Uri navigationUri, ShellTileData initialData)
        {
            
        }

        public void Delete()
        {
            
        }

        public void Update(ShellTileData data)
        {
            
        }
    }
}
