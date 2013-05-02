using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedAgent_0_1
{
    public class Doctor
    {
        public int DocID { get; set; }
        public int MemberID { get; set; }
        public string Speciality { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public char sex { get; set; }
        public string email { get; set; }
        public DateTime bday { get; set; }
        public int ssn { get; set; }
        public char UserType { get; set; }
        public string address { get; set; }
        public int PhoneNumber { get; set; }

    }
}
