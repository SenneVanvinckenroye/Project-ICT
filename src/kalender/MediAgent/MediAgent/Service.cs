using System;
using System.Collections.Generic;

namespace MediAgent
{
    class Service
    {

        private static readonly MobileServiceTable<Patient> PatientTable = App.MobileServiceClient.GetTable<Patient>();

        public static void Insert(string first, string last, int tele, string adres)
        {
            var item = new Patient { FirstName = first, LastName = last, Telephone = tele, Address = adres};
            PatientTable.Insert(item, (res, err) =>
            {
                if (err != null)
                {
                    //handle it
                    System.Diagnostics.Debugger.Break();
                    return;
                }
                item = res;
            });
        }

        public static List<Patient> Lst;

        public static void Read()
        {
            // OData query
            Lst = new List<Patient>();
            Query(null, "Id");
        }
        public static List<Patient> Load()
        {
            if (Lst != null)
            {
                return Lst;
            }
            System.Diagnostics.Debugger.Break();
            return null;
        }
        public static void Delete(Patient item)
        {
            if (item != null)
            {
                PatientTable.Delete(item.Id, err =>
                    {
                        if (err != null)
                        {
                            //handle it
                            System.Diagnostics.Debugger.Break();
                        }
                    });
            }
        }

        public static event EventHandler DownloadDone;
        public static void Query(string query1, string orderby)
        {
            // OData query http://www.odata.org/documentation/uri-conventions#FilterSystemQueryOption
            Lst = new List<Patient>();
            var query = new MobileServiceQuery()
                .Filter(query1)
                .OrderBy(orderby);

            PatientTable.Get(query, (res, err) =>
            {
                if (err != null)
                {
                    //handle it
                    System.Diagnostics.Debugger.Break();
                    return;
                }
                foreach (Patient todo in res)
                {
                    Lst.Add(todo);
                }

                if (DownloadDone != null)
                    DownloadDone(null,EventArgs.Empty);
            });
        }
    }
}
