using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Collections.Generic; // For generic collections like List.
using System.Data.SqlClient;      // For the database connections and objects.



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
                alist.Add(new Model.User() { FName = item.FName, LName = item.LName});
            }

            return alist;
        }

        public List<Model.Patient> GetAllPatientsForDoctor(int DocID)
        {
            try
            {
                DataClasses1DataContext dc = new DataClasses1DataContext();
                List<Model.Patient> alist = new List<Model.Patient>();

                var pat = from p in dc.Patients
                         join u in dc.Users on p.MemberID equals u.MemberID
                         where p.DocID == DocID
                         select new { p.PatientID, u.FName, u.LName, u.sex, u.bday, u.email, u.ssn, u.address, u.PhoneNumber };

                foreach (var item in pat)
                {
                    alist.Add(new Model.Patient() { PatientID = item.PatientID, FirstName = item.FName, LastName = item.LName, bDay = item.bday, Sex = item.sex, Ssn = Convert.ToInt64(item.ssn), Email = item.email, address = Convert.ToString(item.address), phoneNumber = Convert.ToInt64(item.PhoneNumber) });
                }
                //return "error";
                return alist;
            }
            catch(Exception e)
            {
                return null;
                //return "GetAllPatientsError? : \n\r"+e.Message;
            }
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
                string body = "<h1>Welcome! " + PatientFName + "!</h1><br/>Dr. " + DoctorFName + " added you to his MedCare application.<br/>You can now also use this as your personal medication reminder!<br/><br/>Keep in mind that your password is: " + PatientPass + ". Make sure to keep it safe!<br><br><br>Greetings from the MedCare Team!";
                string html = @"
<html>
<head>
</head>
<body>
"+body+@"
</body>
</html>
";
                msg.Body = html;
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
                Ulist.Add(new Model.User() { UserType = i.UserType.Value,FName = i.FName,LName = i.LName,sex = i.sex,email = i.email,MemberID = i.MemberID });
            }
            if (Ulist.Count() > 0)
                return Ulist.First();//altijd maar 1 gebruiker in Ulist mits email uniek is
            else
                return null;
        }
        SqlConnection con;
        public string CreateNewUser(string FName, string LName, string pass_hash, string email, char sex, int docID, char type, DateTime bday, string address, int ssn, int DocID, int phoneNumber)
        {
            try
            {
                ///using SQL to insert new user
                using (con = new SqlConnection("Server=tcp:kqayqahno5.database.windows.net;Database=medplanner-2013-3-15-11-53;User ID=medagent@kqayqahno5;Password=Finland1!;Trusted_Connection=False;"))
                {
                        try
                        {
                            con.Open();
                        }
                        catch
                        {
                            return "SQL Connection Error";
                        }
                        try
                        {
                            using (SqlCommand command = new SqlCommand(
                            "INSERT INTO Users (FName, LName, pass_hash, sex, email, bday, ssn, UserType, address, phoneNumber) VALUES (@FName, @LName, @pass_hash, @sex, @email, @bday, @ssn, @UserType, @address, @phoneNumber)", con))
                            {
                                command.Parameters.Add(new SqlParameter("@FName", FName));
                                command.Parameters.Add(new SqlParameter("@LName", LName));
                                command.Parameters.Add(new SqlParameter("@pass_hash", pass_hash));
                                command.Parameters.Add(new SqlParameter("@email", email));
                                command.Parameters.Add(new SqlParameter("@sex", sex));
                                command.Parameters.Add(new SqlParameter("@UserType", type));
                                command.Parameters.Add(new SqlParameter("@bday", bday));
                                command.Parameters.Add(new SqlParameter("@address", address));
                                command.Parameters.Add(new SqlParameter("@ssn", ssn));
                                command.Parameters.Add(new SqlParameter("@phoneNumber", phoneNumber));
                                command.ExecuteNonQuery();
                                con.Close();
                                con.Dispose();
                            }
                        }
                        catch (SqlException e)
                        {
                            return e.Message;
                        }
                    }

                    DataClasses1DataContext dc = new DataClasses1DataContext();
                    User usr = new User();
                    usr.FName = FName;
                    /*usr.LName = LName;
                    usr.email = email;
                    usr.sex = sex;
                    usr.UserType = type;
                    usr.bday = bday;

                    dc.Users.InsertOnSubmit(usr);
                    try
                    {
                        dc.SubmitChanges();
                    }
                    catch
                    {
                        return 'u';
                    }*/
                    //nu gebruiker selecteren om memberID terug te halen
                    try
                    {
                        var users = from u in dc.Users
                                    where u.email == email
                                    select new { u.MemberID };
                        foreach (var user in users)
                        {
                            usr.MemberID = user.MemberID;//get memberID
                        }
                    }
                    catch
                    {
                        return "Failed to retrieve MemberID";
                    }
                    //nu patient toevoegen met memberID en docID
                    try
                    {
                        using (SqlCommand command = new SqlCommand(
                                    "INSERT INTO Patients (MemberID, DocID) VALUES (@MemberID, @DocID)", con))
                        {
                            command.Parameters.Add(new SqlParameter("@MemberID", usr.MemberID));
                            command.Parameters.Add(new SqlParameter("@DocID", DocID));
                            command.ExecuteNonQuery();
                            con.Close();
                            con.Dispose();
                        }
                    }
                    catch
                    {
                        return "Failed to add to Patients table";
                    }
                    return "k";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
        }

        public string CreatePrescription(string DrugName, DateTime StartDate, DateTime EndDate, int Quantity, TimeSpan Time1, TimeSpan Time2, TimeSpan Time3, TimeSpan Time4, TimeSpan Time5, TimeSpan Time6, string Description, string Course, int PatientID, string Type, char Taken1, char Taken2, char Taken3, char Taken4, char Taken5, char Taken6)
        {
            try
            {
                using (con = new SqlConnection("Server=tcp:kqayqahno5.database.windows.net;Database=medplanner-2013-3-15-11-53;User ID=medagent@kqayqahno5;Password=Finland1!;Trusted_Connection=False;"))
                {
                    try
                    {
                        con.Open();
                    }
                    catch
                    {
                        return "SQL Connection error";
                    }
                    try
                    {
                        using (SqlCommand command = new SqlCommand(
                        "INSERT INTO Prescriptions (StartDate, EndDate, Type, Quantity, DrugName, DDescription, Course, PatientID, Time1, Taken1, Time2, Taken2, Time3, Taken3, Time4, Taken4, Time5, Taken5, Time6, Taken6) VALUES (@StartDate, @EndDate, @Type, @Quantity, @DrugName, @DDescription, @Course, @PatientID, @Time1, @Taken1, @Time2, @Taken2, @Time3, @Taken3, @Time4, @Taken4, @Time5, @Taken5, @Time6, @Taken6)", con))
                        {
                            command.Parameters.Add(new SqlParameter("@StartDate", StartDate));
                            command.Parameters.Add(new SqlParameter("@EndDate", EndDate));
                            command.Parameters.Add(new SqlParameter("@Quantity", Quantity));
                            command.Parameters.Add(new SqlParameter("@Taken1", Taken1));
                            command.Parameters.Add(new SqlParameter("@Taken2", Taken2));
                            command.Parameters.Add(new SqlParameter("@Taken3", Taken3));
                            command.Parameters.Add(new SqlParameter("@Taken4", Taken4));
                            command.Parameters.Add(new SqlParameter("@Taken5", Taken5));
                            command.Parameters.Add(new SqlParameter("@Taken6", Taken6));
                            command.Parameters.Add(new SqlParameter("@Time1", Time1));
                            command.Parameters.Add(new SqlParameter("@Time2", Time2));
                            command.Parameters.Add(new SqlParameter("@Time3", Time3));
                            command.Parameters.Add(new SqlParameter("@Time4", Time4));
                            command.Parameters.Add(new SqlParameter("@Time5", Time5));
                            command.Parameters.Add(new SqlParameter("@Time6", Time6));
                            command.Parameters.Add(new SqlParameter("@DrugName", DrugName));
                            command.Parameters.Add(new SqlParameter("@DDescription", Description));
                            command.Parameters.Add(new SqlParameter("@Course", Course));
                            command.Parameters.Add(new SqlParameter("@PatientID", PatientID));
                            command.Parameters.Add(new SqlParameter("@Type", Type));
                            command.ExecuteNonQuery();
                            con.Close();
                            con.Dispose();
                        }
                    }
                    catch (SqlException e)
                    {
                        return e.Message;
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "success";
        }
        public Model.Patient GetPatientData(int MemberID)
        {
            Model.Patient Patient = new Model.Patient();
            try
            {
                DataClasses1DataContext dc = new DataClasses1DataContext();
                var patient = from p in dc.Patients
                              where p.MemberID == MemberID
                              select new { p.PatientID, p.DocID };

                foreach (var p in patient)
                {
                    Patient.PatientID = p.PatientID;
                    Patient.MemberID = MemberID;
                    Patient.DocID = p.DocID;
                }
            }
            catch
            {
                Model.Patient failPatient = new Model.Patient();
                failPatient.PatientID = -1;
                return failPatient;
            }
            return Patient;
        }

        public List<Model.Prescription> GetPrescriptionsForPatient(int PatientID)
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();
            List<Model.Prescription> prescriptions = new List<Model.Prescription>();

            var pres = from p in dc.Prescriptions
                          where p.PatientID == PatientID
                          select p;

            foreach (var i in pres)
            {
                prescriptions.Add(new Model.Prescription() { PrescriptionID = i.PrescriptionID, StartDate = i.StartDate, EndDate = i.EndDate,
                    PatientID = i.PatientID, Course = i.Course, DDescription = i.DDescription, DrugName = i.DrugName, Quantity = i.Quantity,
                    Taken1 = i.Taken1, Taken2 = (char)i.Taken2, Taken3 = (char)i.Taken3, Taken4 = (char)i.Taken4, Taken5 = (char)i.Taken5, Taken6 = (char)i.Taken6,
                    Time1 = i.Time1, Time2 = (TimeSpan)i.Time2, Time3 = (TimeSpan)i.Time3, Time4 = (TimeSpan)i.Time4, Time5 = (TimeSpan)i.Time5, Time6 = (TimeSpan)i.Time6 });
            }
            if (prescriptions.Count() > 0)
                return prescriptions;
            else
                return null;
        }

        public Model.Doctor GetDocInfo(int MemberID)
        {
            try
            {
                Model.Doctor dokter = new Model.Doctor();
                DataClasses1DataContext dc = new DataClasses1DataContext();
                var Doctors = from d in dc.Doctors
                             where d.MemberID == MemberID
                             select new {d.DocID,d.MemberID,d.Speciality };

                foreach (var doctor in Doctors)
                {
                    dokter.DoctorID = doctor.DocID;
                    dokter.MemberID = doctor.MemberID;
                    dokter.Speciality = doctor.Speciality;
                }
                return dokter;
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
