using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL_App1
{
    public static class MenuManager
    {
        public static MenuChoice Menu(string[] choices, string title = "Make your choice")
        {
            var mc = new MenuChoice();
            mc.choices = choices;
            Console.Clear();
            for (int i = 0; i < choices.Length + 1; i++)
            {
                Console.Write("\r\n");
            }
            Console.WriteLine(title.DrawInConsoleBox());
            //var menu = new Menu(new string[] { "John", "Bill", "Janusz", "Grażyna", "1500", ":)" });
            var menu = new MenuClass(choices);
            var menuPainter = new ConsoleMenuPainter(menu);

            var done = false;

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
            mc.title = title;

            Console.ForegroundColor = ConsoleColor.Cyan;
            //Console.WriteLine("Selected option: " + (menu.SelectedOption ?? "(nothing)"));
            //Console.ReadKey();

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            //return menu.SelectedOption;
            return mc;
        }

        public static MenuChoice Menu(MenuChoice mc)
        {
            Console.Clear();
            for (int i = 0; i < mc.choices.Length + 1; i++)
            {
                Console.Write("\r\n");
            }
            Console.WriteLine(mc.title.DrawInConsoleBox());
            //var menu = new Menu(new string[] { "John", "Bill", "Janusz", "Grażyna", "1500", ":)" });
            var menu = new MenuClass(mc.choices);
            var menuPainter = new ConsoleMenuPainter(menu);

            var done = false;

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

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            //return menu.SelectedOption;
            return mc;
        }

        public static MenuChoice Menu(MenuChoice mc, string title)
        {
            Console.Clear();
            for (int i = 0; i < mc.choices.Length + 1; i++)
            {
                Console.Write("\r\n");
            }
            mc.title = title;
            Console.WriteLine(mc.title.DrawInConsoleBox());
            //var menu = new Menu(new string[] { "John", "Bill", "Janusz", "Grażyna", "1500", ":)" });
            var menu = new MenuClass(mc.choices);
            var menuPainter = new ConsoleMenuPainter(menu);

            var done = false;

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

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            //return menu.SelectedOption;
            return mc;
        }
    }

    public class MenuClass
    {
        public MenuClass(IEnumerable<string> items)
        {
            Items = items.ToArray();
            SelectedIndex = 0;
        }


        public IReadOnlyList<string> Items { get; }

        public int SelectedIndex { get; private set; } = -1; // nothing selected

        public string SelectedOption => SelectedIndex != -1 ? Items[SelectedIndex] : null;


        public void MoveUp()
        {
            SelectedIndex = Math.Max(SelectedIndex - 1, 0);
        }

        public void MoveDown()
        {
            SelectedIndex = Math.Min(SelectedIndex + 1, Items.Count - 1);
        }
    }


    // logic for drawing menu list
    public class ConsoleMenuPainter
    {
        private readonly MenuClass menu;

        public ConsoleMenuPainter(MenuClass menu)
        {
            this.menu = menu;
        }

        public void Paint(int x, int y)
        {
            for (var i = 0; i < menu.Items.Count; i++)
            {
                Console.SetCursorPosition(x, y + i);

                var color = menu.SelectedIndex == i ? ConsoleColor.Black : ConsoleColor.Gray;
                var color_ = menu.SelectedIndex == i ? ConsoleColor.Gray : ConsoleColor.Black;

                Console.ForegroundColor = color;
                Console.BackgroundColor = color_;
                Console.WriteLine(menu.Items[i]);
            }
        }
    }


    public class MenuChoice
    {
        public string[] choices;
        public string choice;
        public string title = "";
        public int choice_id = -1;
        public bool used = false;
    }
}