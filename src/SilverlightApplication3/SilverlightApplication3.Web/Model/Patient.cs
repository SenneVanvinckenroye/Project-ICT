﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SilverlightApplication3.Web.Model
{
    [DataContract]
    public class Patient
    {
        [DataMember]
        public int PatientID { get; set; }

        [DataMember]
        public int MemberID { get; set; }

        [DataMember]
        public int DocID { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public DateTime bDay { get; set; }

        [DataMember]
        public char Sex { get; set; }

        [DataMember]
        public long Ssn { get; set; }//bigint in db

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string address { get; set; }

        [DataMember]
        public long phoneNumber { get; set; }//bigint in db
    }
}