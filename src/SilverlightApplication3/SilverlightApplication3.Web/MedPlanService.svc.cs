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

        public List<Model.Patient> GetAllPatients()
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();
            List<Model.Patient> alist = new List<Model.Patient>();

            var us = from p in dc.Patients
                     join u in dc.Users on p.MemberID equals u.MemberID
                     select new { p.PatientID, u.FName, u.LName };

            foreach (var item in us)
            {
                alist.Add(new Model.Patient() { PatientID = item.PatientID, FirstName = item.FName, LastName = item.LName });
            }

            return alist;
        }


        public string Login(string email, string pswd_hash)
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();
            List<Model.User> Ulist = new List<Model.User>();

            pswd_hash = CreateMD5Hash(pswd_hash);

            /*var user = from u in dc.Users
                       where u.email == email && u.pass_hash == pswd_hash
                       select new { u.FName, u.LName };*/

            var user = from u in dc.Users
                          where u.email == email && u.pass_hash == pswd_hash
                          select new { u.UserType };

            foreach (var i in user)
            {
                Ulist.Add(new Model.User() { UserType = i.UserType.Value });
            }
            if (Ulist.Count() > 0)
            {
                return user.First().UserType.Value.ToString();
            }
            return "";
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
