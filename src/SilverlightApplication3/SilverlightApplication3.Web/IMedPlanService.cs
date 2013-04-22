using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SilverlightApplication3.Web
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMedPlanService" in both code and config file together.
    [ServiceContract]
    public interface IMedPlanService
    {
        [OperationContract]
        void DoWork();

        [OperationContract]  
        List<Model.User> GetAllUsers();

        [OperationContract]
        List<Model.Patient> GetAllPatientsForDoctor(int DocID);

        [OperationContract]
        Model.User Login(string email, string pswd_hash);

        [OperationContract]
        string CreateNewUser(string FName,string LName, string pass_hash, string email, char sex, int docID, char type, DateTime bday, string address, int ssn);//return type char for debugging

        [OperationContract]
        bool SendEmail(string PatientEmail, string PatientFName, string DoctorLName, string PatientPass);
    }
}
