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
                switch (userInput)
                {
                    case 1:
                        RegisterNewUser();
                        break;
                    case 2:
                        PrintTable();
                        break;
                    case 3:
                        Transaction();
                        break;
                    case 4:
                        DataSet();
                        break;
                    default:
                        Environment.Exit(0);
                        break;
                }
            } while (userInput != 0);
        }

        private static void RegisterNewUser()
        {
            string username = GetInput("Username");
            string password = GetInput("Password");

            SqlManager databaseSqlManager = new SqlManager();
            int userID = databaseSqlManager.RegisterUser(username, password);
            Console.WriteLine($"Registered user ID: {userID}");
            Console.ReadLine();
        }

        private static void PrintTable()
        {
            string tableName = GetInput("Table name");
            SqlManager sqlDatabaseManager = new SqlManager();
            sqlDatabaseManager.printTable(tableName);
        }

        private static void Transaction()
        {
            SqlManager manager = new SqlManager();
            manager.Transaction();
        }

        private static void DataSet()
        {
            SqlManager manager = new SqlManager();
            manager.DataSet();
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
            Console.WriteLine("0. EXIT");
            var result = Console.ReadLine();
            return Convert.ToInt32(result);
        }
    }
}
