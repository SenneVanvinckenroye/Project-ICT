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
        string CreateNewUser(string FName, string LName, string pass_hash, string email, char sex, int docID, char type, DateTime bday, string address, int ssn, int DocID, int phoneNumber);//return type char for debugging

        [OperationContract]
        bool SendEmail(string PatientEmail, string PatientFName, string DoctorLName, string PatientPass);

        [OperationContract]
        string CreatePrescription(string DrugName, DateTime StartDarte, DateTime EndDate, int Quantity, TimeSpan Time1, TimeSpan Time2, TimeSpan Time3, TimeSpan Time4, TimeSpan Time5, TimeSpan Time6, string Description, string Course, int PatientID, string Type, char Taken1, char Taken2, char Taken3, char Taken4, char Taken5, char Taken6);

        [OperationContract]
        Model.Patient GetPatientData(int MemberID);

        [OperationContract]
        List<Model.Prescription> GetPrescriptionsForPatient(int PatientID);

        [OperationContract]
        Model.Doctor GetDocInfo(int MemberID);
    }
}
