using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SilverlightApplication3.Web.Model
{
    [DataContract]
    public class Prescription
    {
        [DataMember]
        public int PrescriptionID { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public DateTime EndDate { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public char Taken1 { get; set; }

        [DataMember]
        public char Taken2 { get; set; }

        [DataMember]
        public char Taken3 { get; set; }

        [DataMember]
        public char Taken4 { get; set; }

        [DataMember]
        public char Taken5 { get; set; }

        [DataMember]
        public char Taken6 { get; set; }

        [DataMember]
        public string Course { get; set; }

        [DataMember]
        public int PatientID { get; set; }

        [DataMember]
        public string DrugName { get; set; }

        [DataMember]
        public string DDescription { get; set; }

        [DataMember]
        public TimeSpan Time1 { get; set; }

        [DataMember]
        public TimeSpan Time2 { get; set; }

        [DataMember]
        public TimeSpan Time3 { get; set; }

        [DataMember]
        public TimeSpan Time4 { get; set; }

        [DataMember]
        public TimeSpan Time5 { get; set; }

        [DataMember]
        public TimeSpan Time6 { get; set; }



    }
}