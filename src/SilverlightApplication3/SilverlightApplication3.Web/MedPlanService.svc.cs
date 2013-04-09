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

        public List<Model.Patient> GetAllPatientsForDocter(int DocID)
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();
            List<Model.Patient> alist = new List<Model.Patient>();

            var us = from p in dc.Patients
                     join u in dc.Users on p.MemberID equals u.MemberID
                     where p.DocID == DocID
                     select new { p.PatientID, u.FName, u.LName };

            foreach (var item in us)
            {
                alist.Add(new Model.Patient() { PatientID = item.PatientID, FirstName = item.FName, LastName = item.LName });
            }

            return alist;
        }


        public Model.User Login(string email, string pswd_hash)//email en lokaal gehashed paswoord als params
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();
            List<Model.User> Ulist = new List<Model.User>();

            var user = from u in dc.Users
                          where u.email == email && u.pass_hash == pswd_hash
                          select new { u.FName,u.LName,u.MemberID,u.UserType,u.sex,u.email };

            foreach (var i in user)
            {
                Ulist.Add(new Model.User() { UserType = i.UserType.Value,FirstName = i.FName,Name = i.LName,sex = i.sex,email = i.email,memberID = i.MemberID });
            }
            if (Ulist.Count() > 0)
                return Ulist.First();//altijd maar 1 gebruiker in Ulist mits email uniek is
            else
                return null;
        }
    }
}
