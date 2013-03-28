using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace SilverlightApplication3.Web
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MedPlanService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MedPlanService.svc or MedPlanService.svc.cs at the Solution Explorer and start debugging.
    public class MedPlanService : IMedPlanService
    {
        public void DoWork()
        {
        }


        public List<Model.User> GetAllUsers()
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();
            List<Model.User> alist = new List<Model.User>();
            
            var us = from u in dc.Users
                     select new { u.FName, u.LName};

            foreach (var item in us)
            {
                alist.Add(new Model.User() { FirstName = item.FName, Name = item.LName});
            }

            return alist;
        }

        public string Login(string email, string pswd_hash)
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();
            List<Model.User> alist = new List<Model.User>();

            var patient = from u in dc.Users
                          join p in dc.Patients on u.MemberID equals p.MemberID
                          where u.email == email && u.pass_hash == pswd_hash
                          select new { p.PatientID };

            var doctor = from u in dc.Users
                         join d in dc.Doctors on u.MemberID equals d.MemberID
                         where u.email == email && u.pass_hash == pswd_hash
                         select new { d.DocID };

            var nurse = from u in dc.Users
                        join n in dc.Nurses on u.MemberID equals n.MemberID
                        where u.email == email && u.pass_hash == pswd_hash
                        select new { n.NurseID };

            if (patient.FirstOrDefault().PatientID != 0)
            {
                foreach (var i in patient)
                {
                    alist.Add(new Model.User() { memberID = i.PatientID });
                }
                
                return "patient";
            }
            else if (doctor.FirstOrDefault().DocID != 0)
            {
                foreach (var i in doctor)
                {
                    alist.Add(new Model.User() { memberID = i.DocID });
                }

                return "doctor";
            }
            else if (nurse.FirstOrDefault().NurseID != 0)
            {
                foreach (var i in nurse)
                {
                    alist.Add(new Model.User() { memberID = i.NurseID });
                }

                return "nurse";
            }
            else
            {
                return "";
            }
        }

        public string CreateMD5Hash(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
                // To force the hex string to lower-case letters instead of
                // upper-case, use he following line instead:
                // sb.Append(hashBytes[i].ToString("x2")); 
            }
            return sb.ToString();
        }
    }
}
