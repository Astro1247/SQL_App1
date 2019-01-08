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
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SQL_App1
{


    public static class Core
    {

        #region Crit

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

        #endregion

        static void Main(string[] args)
        {
            SqlConnection conn = new SqlConnection();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

            conn.Open();
            SqlCommand command = new SqlCommand();
            command.Connection = conn;

            switch (MenuManager.Menu(new string[] { "SQL management", "LINQ to SQL management", "LINQ to DataSet management", "CodeFirst" }).choice_id)
            {
                case 0:
                    Sql.Init();
                    break;
                case 1:
                    LinqToSql.Init();
                    break;
                case 2:
                    LinqToDataSet.Init();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Non implemented yet.");
                    break;
            }

            

            //Crit();
            //LinqToSql linq = new LinqToSql();
            //linq.Init();
        }

        public static string DrawInConsoleBox(this string s)
        {
            string ulCorner = "╔";
            string llCorner = "╚";
            string urCorner = "╗";
            string lrCorner = "╝";
            string vertical = "║";
            string horizontal = "═";

            string[] lines = s.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);


            int longest = 0;
            foreach (string line in lines)
            {
                if (line.Length > longest)
                    longest = line.Length;
            }
            int width = longest + 2; // 1 space on each side


            string h = string.Empty;
            for (int i = 0; i < width; i++)
                h += horizontal;

            // box top
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ulCorner + h + urCorner);

            // box contents
            foreach (string line in lines)
            {
                double dblSpaces = (((double)width - (double)line.Length) / (double)2);
                int iSpaces = Convert.ToInt32(dblSpaces);

                if (dblSpaces > iSpaces) // not an even amount of chars
                {
                    iSpaces += 1; // round up to next whole number
                }

                string beginSpacing = "";
                string endSpacing = "";
                for (int i = 0; i < iSpaces; i++)
                {
                    beginSpacing += " ";

                    if (!(iSpaces > dblSpaces && i == iSpaces - 1)) // if there is an extra space somewhere, it should be in the beginning
                    {
                        endSpacing += " ";
                    }
                }
                // add the text line to the box
                sb.AppendLine(vertical + beginSpacing + line + endSpacing + vertical);
            }

            // box bottom
            sb.AppendLine(llCorner + h + lrCorner);

            // the finished box
            return sb.ToString();
        }

        public static string GetInput(string inputName)
        {
            Console.Write($"{inputName}: ");
            return Console.ReadLine();
        }
    }
}
