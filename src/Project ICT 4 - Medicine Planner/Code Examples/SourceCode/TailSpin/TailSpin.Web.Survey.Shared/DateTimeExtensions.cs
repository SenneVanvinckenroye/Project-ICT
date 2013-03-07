//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Web.Survey.Shared
{
    using System;

    public static class DateTimeExtensions
    {
        public static string GetFormatedTicks(this DateTime dateTime)
        {
            return string.Format("{0:D19}", dateTime.Ticks);
        }
    }
}
