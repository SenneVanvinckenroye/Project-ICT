using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SilverlightApplication3.Web.Model
{
    [DataContract]
    public class Doctor
    {
        [DataMember]
        public int DoctorID { get; set; }

        [DataMember]
        public int MemberID { get; set; }

        [DataMember]
        public string Speciality { get; set; }

    }
}