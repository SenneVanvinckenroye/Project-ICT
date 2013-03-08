using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoneApp1
{
    class Service
    {

        private static MobileServiceTable<TodoItem> todoItemTable = App.MobileServiceClient.GetTable<TodoItem>();

        public static void Insert(string data)
        {
            var item = new TodoItem { Text = data };
            todoItemTable.Insert(item, (res, err) =>
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

        public static List<TodoItem> lst;

        public static void Read()
        {
            // OData query
            lst = new List<TodoItem>();
            todoItemTable.GetAll((res, err) =>
            {
                if (err != null)
                {
                    //handle it
                    System.Diagnostics.Debugger.Break();
                    return;
                }
                foreach (TodoItem todo in res)
                {
                    lst.Add(todo);
                }
            });
        }
        public static List<TodoItem> Load()
        {
            if (lst != null)
            {
                return lst;
            }
            else
            {
                System.Diagnostics.Debugger.Break();
                return null;
            }
        }
        public static void Delete(TodoItem item)
        {
            todoItemTable.Delete(item.Id, err =>
            {
                if (err != null)
                {
                    //handle it
                    System.Diagnostics.Debugger.Break();
                }
            });
        }
        public static void Query(string query1)
        {
            // OData query http://www.odata.org/documentation/uri-conventions#FilterSystemQueryOption
            lst = new List<TodoItem>();
            var query = new MobileServiceQuery()
                .Filter(query1);

            todoItemTable.Get(query, (res, err) =>
            {
                if (err != null)
                {
                    //handle it
                    System.Diagnostics.Debugger.Break();
                    return;
                }
                foreach (TodoItem todo in res)
                {
                    lst.Add(todo);
                }
            });
        }
    }
}
