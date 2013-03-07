//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Web.Survey.Shared.SurveysFiltering
{
    using System.Collections.Generic;
    using System.Globalization;
    using TailSpin.Web.Survey.Shared.Models;

    public class SurveysComparer : IEqualityComparer<Survey>
    {
        public bool Equals(Survey x, Survey y)
        {
            return
                x.SlugName == y.SlugName &&
                x.Tenant == y.Tenant;
        }

        public int GetHashCode(Survey obj)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0}{1}",
                obj.Tenant,
                obj.SlugName)
                .GetHashCode();
        }
    }
}