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
        public string Name { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public Int32 memberID { get; set; }

        [DataMember]
        public string psw_hash { get; set; }

        [DataMember]
        public char sex { get; set; }

        [DataMember]
        public string email { get; set; }

        [DataMember]
        public DateTime date { get; set; }

        [DataMember]
        private string userType { get; set; }
    }
}