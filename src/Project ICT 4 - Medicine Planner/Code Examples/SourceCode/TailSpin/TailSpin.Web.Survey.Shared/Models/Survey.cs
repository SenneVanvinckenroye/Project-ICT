//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Web.Survey.Shared.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [Serializable]
    public class Survey
    {
        private readonly string slugName;

        public Survey()
            : this(string.Empty)
        {
        }

        public Survey(string slugName)
        {
            this.slugName = slugName;
            this.Questions = new List<Question>();
        }

        public string SlugName
        {
            get
            {
                return this.slugName;
            }
        }

        public string Tenant { get; set; }

        [Required(ErrorMessage = "* You must provide a Title for the survey.")]
        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }
        
        public List<Question> Questions { get; set; }
    }
}

