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
    public class ShellTileAdapter : IShellTile
    {
        public ShellTileAdapter(){}

        public ShellTileAdapter(ShellTile tile)
        {
            WrappedSubject = tile;
        }

        private ShellTile WrappedSubject { get; set; }

        public void Update(ShellTileData data)
        {
            WrappedSubject.Update(data);
        }

        public void Create(Uri navigationUri, ShellTileData initialData)
        {
            ShellTile.Create(navigationUri, initialData);
        }

        public void Delete()
        {
            WrappedSubject.Delete();
        }

        public IEnumerable<ShellTile> ActiveTiles
        {
            get { return ShellTile.ActiveTiles; }
        }

        public Uri NavigationUri
        {
            get { return WrappedSubject.NavigationUri; }
        }
    }
}
