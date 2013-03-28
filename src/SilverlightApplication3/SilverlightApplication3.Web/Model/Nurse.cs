using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SilverlightApplication3.Web.Model
{
    [DataContract]
    public class Nurse
    {
        [DataMember]
        public int NurseID { get; set; }

        [DataMember]
        public int MemberID { get; set; }

    }
}