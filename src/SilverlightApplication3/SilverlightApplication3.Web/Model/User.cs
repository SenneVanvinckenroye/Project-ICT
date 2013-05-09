using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SilverlightApplication3.Web.Model
{
    [DataContract]
    public class User
    {
        [DataMember]
        public string LName { get; set; }

        [DataMember]
        public string FName { get; set; }

        [DataMember]
        public Int32 MemberID { get; set; }

        [DataMember]
        public string pass_hash { get; set; }

        [DataMember]
        public char sex { get; set; }

        [DataMember]
        public string email { get; set; }

        [DataMember]
        public DateTime bday { get; set; }

        [DataMember]
        public long ssn { get; set; }

        [DataMember]
        public char UserType { get; set; }

        [DataMember]
        public string address { get; set; }

        [DataMember]
        public string phoneNumber { get; set; }//varchar in db
    }
}