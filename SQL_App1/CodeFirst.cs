using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQL_App1.DataModel;

namespace SQL_App1
{
    class CodeFirst
    {
        public void Init()
        {
            using (BaseContext cntx = new BaseContext())
            {
                //MenuChoice CodeFirstMenu = new MenuChoice();
                //MenuChoice MainMenu = MenuManager.Menu(new string[] { "Create new chat", "Print \"accounts\" table", "Transaction", "Exit" }, "Messenger example usage Linq to Sql");
                MenuChoice CodeFirstMenu = MenuManager.Menu(new string[]
                    {"Register user", "Get users", "Edit user", "Delete user"});
                while (CodeFirstMenu.choice != "Exit")
                {
                    switch (CodeFirstMenu.choice)
                    {
                        case "Register user":
                            RegisterUser(cntx);
                            break;
                        case "Get users":
                            GetUsers(cntx);
                            break;
                        case "Edit user":
                            EditUser(cntx);
                            break;
                        case "Delete user":
                            DeleteUser(cntx);
                            break;
                    }

                    Console.ReadKey(true);
                    MenuManager.Menu(CodeFirstMenu);
                }
            }
        }

        private void EditUser(BaseContext cntx)
        {
            Dictionary<int, string> users = new Dictionary<int, string>();
            foreach (Users cntxUser in cntx.Users)
            {
                users.Add(cntxUser.Id, cntxUser.Username);
            }

            string[] usernames = new string[users.Values.Count];
            users.Values.CopyTo(usernames, 0);
            MenuChoice usersMenuChoice = MenuManager.Menu(usernames, "Choose what user you want to edit");

            MenuChoice EditUserMenu = MenuManager.Menu(new string[] {"Edit username", "Edit password", "Go back"});
            while (EditUserMenu.choice != "Go back")
            {
                var query = (from e in cntx.Users
                    where e.Username == usersMenuChoice.choice
                    orderby e.Id ascending
                    select e).FirstOrDefault();

                switch (EditUserMenu.choice)
                {
                    case "Edit username":
                        EditUsername();
                        break;
                    case "Edit password":
                        EditPassword();
                        break;
                }

                void EditUsername()
                {
                    Console.Clear();
                    Console.WriteLine($"Current username: {usersMenuChoice.choice}");
                    string newUsername = Core.GetInput("New username");
                    Console.WriteLine("In process . . .");
                    query.Username = newUsername;
                    usersMenuChoice.choice = newUsername;
                }

                void EditPassword()
                {
                    Console.Clear();
                    Console.WriteLine($"Current password: {cntx.Users.Where(u => u.Username == usersMenuChoice.choice).FirstOrDefault().Password}");
                    string newPassword = Core.GetInput("New password");
                    Console.WriteLine("In process . . .");
                    query.Password = newPassword;
                }

                try
                {
                    cntx.SaveChanges();
                    Console.WriteLine(
                        $"{(EditUserMenu.choice == "Edit username" ? "Username" : EditUserMenu.choice == "Edit password" ? "Password" : "undefined")} for user '{usersMenuChoice.choice}' was successfully changed!"
                            .DrawInConsoleBox());
                    Console.WriteLine("Press any key to continue . . .");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine("Conflict detected!".DrawInConsoleBox());
                    foreach (DbEntityEntry exEntry in ex.Entries)
                    {
                        var dbValues = exEntry.GetDatabaseValues();
                        exEntry.OriginalValues.SetValues(dbValues);
                    }
                }

                Console.ReadKey(true);
                MenuManager.Menu(EditUserMenu);
            }
        }

        private void GetUsers(BaseContext cntx)
        {
            var users = cntx.Users;
            Console.WriteLine("ID\tUSERNAME\tPASSWORD");
            foreach (Users user in users)
            {
                Console.WriteLine($"{user.Id}\t{user.Username}\t{user.Password}");
            }

            Console.WriteLine("\r\n\r\n" +
                              "Press any key to continue . . .");
        }

        private void DeleteUser(BaseContext cntx)
        {
            Dictionary<int, string> users = new Dictionary<int, string>();
            foreach (Users cntxUser in cntx.Users)
            {
                users.Add(cntxUser.Id, cntxUser.Username);
            }

            string[] usernames = new string[users.Values.Count];
            users.Values.CopyTo(usernames, 0);
            MenuChoice usersMenuChoice = MenuManager.Menu(usernames, "Choose which user you want delete");
            Console.WriteLine("In process . . .");
            Users cntxUsers = cntx.Users
                .Where(u => u.Username == usersMenuChoice.choice)
                .FirstOrDefault();
            cntx.Users.Remove(cntxUsers);
            cntx.SaveChanges();
            Console.WriteLine($"User '{usersMenuChoice.choice}' was successfully deleted!".DrawInConsoleBox());
            Console.WriteLine("Press any key to continue . . .");

        }

        private void RegisterUser(BaseContext cntx)
        {
            Users user = new Users() {Username = Core.GetInput("Username"), Password = Core.GetInput("Password")};
            Console.WriteLine("In process. . .");
            cntx.Users.Add(user);
            cntx.SaveChanges();
            Console.WriteLine("User registered!".DrawInConsoleBox());



        }
    }
}
