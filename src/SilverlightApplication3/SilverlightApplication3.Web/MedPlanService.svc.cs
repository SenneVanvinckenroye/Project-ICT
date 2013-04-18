using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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

        public List<Model.Patient> GetAllPatientsForDoctor(int DocID)
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();
            List<Model.Patient> alist = new List<Model.Patient>();

            var us = from p in dc.Patients
                     join u in dc.Users on p.MemberID equals u.MemberID
                     where p.DocID == DocID
                     select new { p.PatientID, u.FName, u.LName, u.sex, u.bday, u.email, u.ssn };

            foreach (var item in us)
            {
                alist.Add(new Model.Patient() { PatientID = item.PatientID, FirstName = item.FName, LastName = item.LName, bDay = item.bday, Sex = item.sex, Ssn = (int)item.ssn, Email = item.email });
            }

            return alist;
        }

        public bool SendEmail(string PatientEmail, string PatientFName, string DoctorFName, string PatientPass)
        {
            bool success = false;

            try
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("projectict4@outlook.com");
                msg.To.Add(new MailAddress(PatientEmail));
                msg.Subject = "Welcome to MedCare!";
                msg.Body = "Welcome! " + PatientFName + "!<br/>Dr. "+DoctorFName+" added you to his MedCare application.<br/>You can now also use this as your personal medication reminder!<br/><br/>Keep in mind that your password is: "+PatientPass+". Make sure to keep it safe!<br><br><br>Greetings from the MedCare Team!";
                msg.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.live.com"; // Replace with your servers IP address 
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("projectict4@outlook.com", "Finland1!");
                smtp.EnableSsl = true;
                smtp.Timeout = 5000;
                smtp.Send(msg);
                success = true;
            }

            catch
            {
                success = false;
            }
            return success;
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
