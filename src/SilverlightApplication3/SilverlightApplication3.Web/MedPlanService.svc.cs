using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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

            var user = from u in dc.Users
                       where u.email == email && u.pass_hash == pswd_hash
                       select new { u.FName };

            foreach (var item in user)
            {
                alist.Add(new Model.User() { FirstName = item.FName });
            }
            if (alist.Count > 0)
            {
                return alist.First().FirstName;
            }
            else
                return "";
        }
    }
}
