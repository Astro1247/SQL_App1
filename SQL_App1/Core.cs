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

            switch (Menu(new string[] { "SQL management", "LINQ management" }).choice_id)
            {
                case 0:
                    Sql.Init();
                    break;
            }

            

            //Crit();
            Linq linq = new Linq();
            //linq.Init();
        }

        public static string GetInput(string inputName)
        {
            Console.Write($"{inputName}: ");
            return Console.ReadLine();
        }


        private static MenuChoice Menu(string[] choices, string title = "")
        {
            MenuChoice mc = new MenuChoice();
            mc.choices = choices;
            Console.WriteLine(title);
            Console.Clear();
            //var menu = new Menu(new string[] { "John", "Bill", "Janusz", "Grażyna", "1500", ":)" });
            var menu = new MenuClass(choices);
            var menuPainter = new ConsoleMenuPainter(menu);

            bool done = false;

            do
            {
                menuPainter.Paint(0, 0);

                var keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        menu.MoveUp();
                        break;
                    case ConsoleKey.DownArrow:
                        menu.MoveDown();
                        break;
                    case ConsoleKey.Enter:
                        done = true;
                        break;
                }
            } while (!done);

            mc.choice_id = menu.SelectedIndex;
            mc.choice = menu.SelectedOption;
            mc.used = true;

            Console.ForegroundColor = ConsoleColor.Cyan;
            //Console.WriteLine("Selected option: " + (menu.SelectedOption ?? "(nothing)"));
            //Console.ReadKey();
            Console.Clear();
            //return menu.SelectedOption;
            return mc;
        }
    }
}
