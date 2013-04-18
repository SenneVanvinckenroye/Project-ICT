using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedAgent_0_1
{
    public class Course
    {
        public MedSoort[] MedSoort { get; set; }
    }
    public class MedSoort
    {
        public MedAlarm[] MedAlarm { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class MedAlarm
    {
        public DateTime DateTime { get; set; }
        public bool Taken { get; set; }
    }
}
