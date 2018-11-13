using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic.Devices;

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SQL_App1
{
    public class Core
    {

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);

        static void Crit()
        {
            int isCritical = 1;  // we want this to be a Critical Process
            int BreakOnTermination = 0x1D;  // value for BreakOnTermination (flag)

            Process.EnterDebugMode();  //acquire Debug Privileges

            // setting the BreakOnTermination = 1 for the current process
            NtSetInformationProcess(Process.GetCurrentProcess().Handle, BreakOnTermination, ref isCritical, sizeof(int));
        }

        static void Main(string[] args)
        {
            //Crit();
            Linq linq = new Linq();
            linq.Init();
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

                Console.ReadLine();
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
