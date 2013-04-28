using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedAgent_0_1
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public char Sex { get; set; }
        public string Email { get; set; }
        public DateTime Bday { get; set; }
        public int SSN { get; set; }

        public int Telephone { get; set; }
        public string Address { get; set; }
        public string MedicineHistory { get; set; }
        public List<string> Symptoms { get; set; }

        public override string ToString()
        {
            return LastName + ", " + FirstName + ",  Id: " + Id;
        }
    }
}
