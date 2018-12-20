using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL_App1
{
    public class Sql
    {
        public static void Init()
        {
            int userInput = 0;
            do
            {
                //userInput = DrawSqlMenu();
                SqlManager manager = new SqlManager();
                /*            Console.WriteLine("SQL console usage application #1\r\n");
                   Console.WriteLine("1. Register new user");
                   Console.WriteLine("2. Print table by Name");
                   Console.WriteLine("3. Transaction");
                   Console.WriteLine("4. DataSet");
                   Console.WriteLine("5. DataReader (getUsers)");
                   Console.WriteLine("0. EXIT");*/
                //switch (userInput)
                switch (MenuManager.Menu(new string[] { "1. Register new user", "2. Print table by Name", "3. Transaction", "4. DataSet", "5. DataReader (getUsers)", "0. EXIT" }, "SQL console usage application #1").choice_id)
                {
                    case 0:
                        RegisterNewUser(manager);
                        break;
                    case 1:
                        PrintTable(manager);
                        break;
                    case 2:
                        Transaction(manager);
                        break;
                    case 3:
                        DataSet(manager);
                        break;
                    case 4:
                        GetUsers(manager);
                        break;
                    case 5:
                    default:
                        Environment.Exit(0);
                        break;
                }

                Console.ReadLine();
            } while (userInput != -1);
        }

        private static void RegisterNewUser(SqlManager manager)
        {
            string username = Core.GetInput("Username");
            string password = Core.GetInput("Password");

            int userID = manager.RegisterUser(username, password);
            Console.WriteLine($"Registered user ID: {userID}");
            //Console.ReadLine();
        }

        private static void PrintTable(SqlManager manager)
        {
            //string tableName = Core.GetInput("Table name");

            //manager.printTable(tableName);
            manager.printTable(MenuManager.Menu(new string[] {"accounts", "Keys"}, "Choose table").choice);
        }

        private static void Transaction(SqlManager manager)
        {
            manager.Transaction();
        }

        private static void DataSet(SqlManager manager)
        {
            manager.DataSet();
        }

        private static void GetUsers(SqlManager manager)
        {
            manager.GetUsers();
        }



        public static int DrawSqlMenu()
        {
            Console.Clear();
            //Console.WriteLine(BigInteger.);
            Console.WriteLine("SQL console usage application #1\r\n");
            Console.WriteLine("1. Register new user");
            Console.WriteLine("2. Print table by Name");
            Console.WriteLine("3. Transaction");
            Console.WriteLine("4. DataSet");
            Console.WriteLine("5. DataReader (getUsers)");
            Console.WriteLine("0. EXIT");
            var result = Console.ReadLine();
            return Convert.ToInt32(result);
        }
    }
}
