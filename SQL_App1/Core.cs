using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL_App1
{
    public class Core
    {
        static void Main(string[] args)
        {
            int userInput = 0;
            do
            {
                userInput = DrawMenu();
                SqlManager manager = new SqlManager();
                switch (userInput)
                {
                    case 1:
                        RegisterNewUser(manager);
                        break;
                    case 2:
                        PrintTable(manager);
                        break;
                    case 3:
                        Transaction(manager);
                        break;
                    case 4:
                        DataSet(manager);
                        break;
                    case 5:
                        GetUsers(manager);
                        break;
                    default:
                        Environment.Exit(0);
                        break;
                }
            } while (userInput != 0);
        }

        private static void RegisterNewUser(SqlManager manager)
        {
            string username = GetInput("Username");
            string password = GetInput("Password");

            int userID = manager.RegisterUser(username, password);
            Console.WriteLine($"Registered user ID: {userID}");
            Console.ReadLine();
        }

        private static void PrintTable(SqlManager manager)
        {
            string tableName = GetInput("Table name");

            manager.printTable(tableName);
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

        public static string GetInput(string inputName)
        {
            Console.Write($"{inputName}: ");
            return Console.ReadLine();
        }

        public static int DrawMenu()
        {
            Console.Clear();
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
