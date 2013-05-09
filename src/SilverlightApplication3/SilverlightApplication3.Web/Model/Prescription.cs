using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml.Linq;

namespace SilverlightApplication3.Web.Model
{
    [DataContract]
    public class Prescription
    {
        [DataMember]
        public int PrescriptionID { get; set; }

        [DataMember]
        public int PatientID { get; set; }

        [DataMember]
        public string data { get; set; }



    }
}