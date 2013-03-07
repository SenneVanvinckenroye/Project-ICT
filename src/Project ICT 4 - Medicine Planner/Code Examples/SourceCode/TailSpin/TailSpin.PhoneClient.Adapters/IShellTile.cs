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

namespace TailSpin.PhoneClient.Adapters
{
    public interface IShellTile
    {
        IEnumerable<ShellTile> ActiveTiles { get; }
        Uri NavigationUri { get; }

        void Create(Uri navigationUri, ShellTileData initialData);
        void Delete();
        void Update(ShellTileData data);    
    }
}