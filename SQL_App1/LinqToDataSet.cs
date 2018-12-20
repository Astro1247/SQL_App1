using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SQL_App1
{
    class LinqToDataSet
    {
        static string ConnString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

        public static void Init()
        {
            DataSet ds = new DataSet();
            FillDs(ds);

            DataTable Accounts = ds.Tables["accounts"];
            DataTable Chats = ds.Tables["chats"];
            DataTable Messages = ds.Tables["messages"];
            DataTable keys = ds.Tables["Keys"];

            var LinqTypeMenu = MenuManager.Menu(new string[] {"Declarational LINQ style", "OOP LINQ style", "Exit"},
                "Please, select LINQ style");
            while (LinqTypeMenu.choice != "Exit")
            {
                switch (LinqTypeMenu.choice_id)
                {
                    case 0:
                        DeclarativeLinq(ds);
                        break;
                    case 1:
                        OopLinq(ds);
                        break;
                }

                MenuManager.Menu(LinqTypeMenu);
            }
        }

        private static void DeclarativeLinq(DataSet ds)
        {
            DataTable Accounts = ds.Tables["accounts"];
            DataTable Chats = ds.Tables["chats"];
            DataTable Messages = ds.Tables["messages"];
            DataTable keys = ds.Tables["Keys"];
            var MainMenu = MenuManager.Menu(new string[] {"Usual query", "Group by", "Union", "Sort", "Exit"},
                "LINQ in declarative style menu");
            while (MainMenu.choice != "Exit")
            {
                switch (MainMenu.choice_id)
                {
                    case 0:
                        GetTable(Accounts);
                        break;
                    case 1:
                        GroupBy(Accounts);
                        break;
                    case 2:
                        Union(Accounts, keys);
                        break;
                    case 3:
                        Sort(keys);
                        break;
                }

                Console.ReadKey(true);
                MenuManager.Menu(MainMenu);
            }
        }

        private static void Sort(DataTable keys)
        {
            var result = keys.AsEnumerable().OrderBy(p => p.Field<Int32>("ownerId"));

            DispTable(result);
        }

        private static void DispTable(OrderedEnumerableRowCollection<DataRow> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine("{0,-15} \t{1,-15} \t{2,-15}", item.Field<Int32>("Id"), item.Field<Int32>("ownerId"), item.Field<String>("key"));

            }
        }

        public static void Union(DataTable accounts, DataTable keys)
        {
            var query = accounts.AsEnumerable().Join(keys.AsEnumerable(),
                p => p.Field<Int32>("Id"),
                c => c.Field<Int32>("ownerId"),
                (p, c) => new
                {
                    Id = p.Field<Int32>("Id"),
                    username = p.Field<String>("username"),
                    password = p.Field<String>("password"),
                    key = c.Field<String>("key")
                });
            Console.WriteLine("UserId   \tUsername   \tPassword   \tKey");
            foreach (var item in query)
            {
                Console.WriteLine("{0,-15} {1,-15} {2,-15} {3,-15}", item.Id, item.username, item.password, item.key);
            }
        }

        public static void GroupBy(DataTable accounts)
        {
            // Group by passwords
            var result = accounts.AsEnumerable().GroupBy(a => a.Field<String>("password"));
            foreach (var list in result)
            {
                Console.WriteLine("Users using password: {0}", list.Key);
                foreach (var item in list)
                    Console.WriteLine(item.Field<String>("username"));
                Console.WriteLine("     <<====================>>     ");
            }
        }

        private static void GetTable(DataTable accounts)
        {
            // После некоторых переносов между пк и типами бд все новые пользователи почему-то стали иметь айди 1000 и выше,
            // данный метод выводит всех новых пользователей с айди 1000+
            var result = accounts.AsEnumerable().Where(a => a.Field<int>("Id") > 1000);
            DispTable(result);
        }

        private static void DispTable(EnumerableRowCollection<DataRow> result)
        {
            foreach (var item in result)
            {
                Console.WriteLine("{0,-15} \t{1,-15} \t{2,-15}", item.Field<int>("Id"), item.Field<String>("username"), item.Field<String>("password"));
            }
        }

        private static void OopLinq(DataSet ds)
        {
            DataTable Accounts = ds.Tables["accounts"];
            DataTable Chats = ds.Tables["chats"];
            DataTable Messages = ds.Tables["messages"];
            DataTable keys = ds.Tables["Keys"];
            var MainMenu = MenuManager.Menu(new string[] {"Average", "Min and Max", "Count", "Subquery", "Exit"},
                "LINQ in OOP style menu");
            while (MainMenu.choice != "Exit")
            {
                switch (MainMenu.choice_id)
                {
                    case 0:
                        Average(Accounts);
                        break;
                    case 1:
                        MinMax(Accounts);
                        break;
                    case 2:
                        Count(Accounts);
                        break;
                    case 3:
                        SubQuery(Accounts, keys);
                        break;

                }

                Console.ReadKey(true);
                MenuManager.Menu(MainMenu);
            }
        }

        public static void SubQuery(DataTable accs, DataTable keys)
        {
            var result = from acs in accs.AsEnumerable()
                let result2 = from kys in keys.AsEnumerable()
                    where kys.Field<int>("ownerId") == 2
                    select kys.Field<int>("ownerId")
                where result2.Contains(acs.Field<int>("Id"))
                select acs;

            DispTable(result);
        }

        public static void Count(DataTable accounts)
        {
            int count = (from a in accounts.AsEnumerable() select a.Field<int>("Id")).Count();
            Console.WriteLine("Registered users count: {0}", count);
        }

        public static void MinMax(DataTable accounts)
        {
            // DataTable Assort1 = ds.Tables["Ассортимент"];
            var MinUserId = (from a in accounts.AsEnumerable() select a.Field<Int32>("Id")).Min();
            var MaxUserId = (from a in accounts.AsEnumerable() select a.Field<Int32>("Id")).Max();
            Console.WriteLine("Min user Id: ={0}\r\n" +
                              "Max user Id: {1}", MinUserId, MaxUserId);
        }

        public static void Average(DataTable accounts)
        {
            var averageUsersId = (from prod in accounts.AsEnumerable() select prod.Field<Int32>("Id")).Average();
            Console.WriteLine(averageUsersId + $" (Rounded: {Math.Round(averageUsersId)})");
        }

        public static DataSet FillDs(DataSet ds)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(
                    "SELECT * FROM accounts " +
                    "SELECT * FROM chats " +
                    "SELECT * FROM messages " +
                    "SELECT * FROM Keys ",
                    ConnString);
                
                adapter.TableMappings.Add("Table", "accounts");
                adapter.TableMappings.Add("Table1", "chats");
                adapter.TableMappings.Add("Table2", "messages");
                adapter.TableMappings.Add("Table3", "Keys");
                adapter.Fill(ds);
                
                DataTable Accounts = ds.Tables["accounts"];
                DataTable Chats = ds.Tables["chats"];
                DataTable Messages = ds.Tables["messages"];
                DataTable keys = ds.Tables["Keys"];
                DataRelation relation = ds.Relations.Add("keydata",
                    ds.Tables["accounts"].Columns["Id"],
                    ds.Tables["Keys"].Columns["ownerId"]);
                //ds.Relations.Add(relation);
                //Console.ReadKey(true);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey(true);
            }
            return ds;
        }
    }
}
