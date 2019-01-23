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
                    {"Register user", "Get users", "Edit user", "Delete user", "Add key", "Связанная загрузка", "Sim", "Sim2", "Sim3"});
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
                        case "Add key":
                            AddKeyForUser(cntx);
                            break;
                        case "Sim":
                            SimpleMethod(cntx);
                            break;
                        case "Sim2":
                            SimpleMethodTwo(cntx);
                            break;
                        case "Sim3":
                            SimpleMethodTree(cntx);
                            break;
                        case "Связанная загрузка":
                        {
                            MenuChoice LoadMenu =
                                MenuManager.Menu(new string[]
                                    {"Отложенная загрузка", "Прямая загрузка", "Явная загрузка", "Выйти"});
                            while (LoadMenu.choice != "Выйти")
                            {
                                switch (LoadMenu.choice_id)
                                {
                                    case 0:
                                        LazyLoading(cntx);
                                        break;
                                    case 1:
                                        //EagerLoading();
                                        break;
                                    case 2:
                                        //ExplicitLoading();
                                        break;
                                    case 3:
                                        break;
                                }
                                    Console.ReadKey(true);
                                    MenuManager.Menu(LoadMenu);
                            }
                        }
                            break;
                    }

                    Console.ReadKey(true);
                    MenuManager.Menu(CodeFirstMenu);
                }
            }
        }

        private void SimpleMethodTree(BaseContext cntx)
        {
            var users = cntx.Users.ToList();
            foreach (var k in users)
            {
                Console.WriteLine("{0,3} \t{1,5} \t{2,15} \t{3,15}", k.Id, k.Username,
                    k.Password, k.Keys.FirstOrDefault().Key == null ? "undefined" : k.Keys.FirstOrDefault().Key);

            }
        }

        private void SimpleMethodTwo(BaseContext cntx)
        {
            foreach (var options in cntx.ChatOptions)
                Console.WriteLine("{0,3} \t{1,5} \t{2,15} \t{3,15}", options.Id, options.IsHidden, options.AnnounceOnly, options.InviteOnly);
            Console.WriteLine("\r\n   <<==========>>  \r\n");
            foreach (var conf in cntx.ConferenceOptions)
                Console.WriteLine("{0,3} \t{1,5} \t{2,15} \t{3,15} \t{4,15} \t{5,15}", conf.Id, conf.IsHidden, conf.AnnounceOnly, conf.InviteOnly, conf.NotifyOnMemberJoin, conf.Period);
            Console.WriteLine("\r\n   <<==========>>  \r\n");
            foreach (var adm in cntx.AdminOptions)
                Console.WriteLine("{0,3} \t{1,5} \t{2,15} \t{3,15} \t{4,15} \t{5,15} \t{6,15} \t{7,15}", adm.Id, adm.IsHidden, adm.AnnounceOnly, adm.InviteOnly, adm.AdminAllRights, adm.MaxAdminCount, adm.NotifyOnMemberJoin, adm.Period);
            Console.WriteLine("\r\n   <<==========>>  \r\n");
        }

        private void SimpleMethod(BaseContext cntx)
        {
            //cntx.Chats.Add(new Chats() {Creator = 5, Messages = new List<Messages>(), member = 7});
            cntx.ChatOptions.Add(new ChatOptions() {AnnounceOnly = true, InviteOnly = false, IsHidden = true});
            cntx.ConferenceOptions.Add(new AdminOptions() {AnnounceOnly = true, InviteOnly = true, IsHidden = false, AdminAllRights = true, MaxAdminCount = 1, NotifyOnMemberJoin = true, Period = 10});
            cntx.AdminOptions.Add(new AdminOptions() {AdminAllRights = true, InviteOnly = false, MaxAdminCount = 2, NotifyOnMemberJoin = false, Period = 50, AnnounceOnly = false, IsHidden = false});
            cntx.SaveChanges();

            
        }

        private void AddKeyForUser(BaseContext cntx)
        {
            Dictionary<int, string> users = new Dictionary<int, string>();
            foreach (Users cntxUser in cntx.Users)
            {
                users.Add(cntxUser.Id, cntxUser.Username);
            }

            string[] usernames = new string[users.Values.Count];
            users.Values.CopyTo(usernames, 0);
            MenuChoice usersMenuChoice = MenuManager.Menu(usernames, "Choose key owner for creating a new key");
            var user = (from e in cntx.Users
                        where e.Username == usersMenuChoice.choice
                        orderby e.Id ascending
                        select e).FirstOrDefault();

            MenuChoice KeygenMenu = MenuManager.Menu(new string[] {"Generate", "Manual input", "Exit"}, $"Key generating menu for user: {usersMenuChoice.choice}");

            while (KeygenMenu.choice != "Exit")
            {
                switch (KeygenMenu.choice_id)
                {
                    case 0:
                        AddNewKey(user, GenerateKey(), cntx);
                        break;
                    case 1:
                        AddNewKey(user, Core.GetInput("Enter key value"), cntx);
                        break;
                }

                Console.ReadKey(true);
                MenuManager.Menu(KeygenMenu);
            }
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private string GenerateKey()
        {
            return RandomString(16);
        }

        private void AddNewKey(Users user, string generateKey, BaseContext cntx)
        {
            DataModel.Keys key = new DataModel.Keys() {Key = generateKey, OwnerId = user.Id, User = new List<Users>() {user} };
            var query = (from e in cntx.Users
                         where e.Username == user.Username
                         orderby e.Id ascending
                         select e).FirstOrDefault();
            
            cntx.Keys.Add(key);
            var queryKey = (from e in cntx.Keys
                            where e.Key == key.Key
                            orderby e.Id ascending
                            select e).FirstOrDefault();
            //query.Keys.Add(queryKey);
            DataModel.Keys[] arrk = new DataModel.Keys[query.Keys.Count];
            query.Keys.CopyTo(arrk, 0);
            Console.WriteLine($"{query.Keys.Count} {arrk.Length} {arrk.ToArray()}");
            cntx.SaveChanges();
            //queryKey.User.Add(query);
            //cntx.SaveChanges();
            Console.WriteLine($"Successfuly added key '{generateKey}' for user '{user.Username}'".DrawInConsoleBox());
        }

        //Отложенная загрузка (lazy loading) заключается в том, что Entity Framework автоматически загружает данные,
        //при этом не загружая связанные данные. Когда потребуются связанные данные Entity Framework создаст еще один запрос к базе данных.
        public static void LazyLoading(BaseContext cntx)
        {
            Dictionary<int, string> users = new Dictionary<int, string>();
            foreach (Users cntxUser in cntx.Users)
            {
                users.Add(cntxUser.Id, cntxUser.Username);
            }

            string[] usernames = new string[users.Values.Count];
            users.Values.CopyTo(usernames, 0);
            MenuChoice usersMenuChoice = MenuManager.Menu(usernames, "Choose user to load");

            var user = (from e in cntx.Users
                         where e.Username == usersMenuChoice.choice
                         orderby e.Id ascending
                         select e).FirstOrDefault();

            // Попытаться загрузить связанные с ним ключи
            if (user != null && user.Keys != null)
                for (int i = 0; i < user.Keys.Count; i++)
                {
                    DataModel.Keys key = user.Keys.ElementAt(i);
                    if (key != null) Console.WriteLine($"Key: {key.Key}");
                }

            Console.WriteLine();
            Console.WriteLine($"All existing keys owned by '{user.Username}'".DrawInConsoleBox());
            Console.ReadKey(true);
            //                foreach (var key in user.Keys)
            //                    Console.WriteLine("Key: " + key.Key);

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
                        EditUsername(cntx, query, usersMenuChoice);
                        break;
                    case "Edit password":
                        EditPassword(cntx, query, usersMenuChoice);
                        break;
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

        private void EditUsername(BaseContext cntx, Users query, MenuChoice usersMenuChoice)
        {
            Console.Clear();
            Console.WriteLine($"Current username: {usersMenuChoice.choice}");
            string newUsername = Core.GetInput("New username");
            Console.WriteLine("In process . . .");
            query.Username = newUsername;
            usersMenuChoice.choice = newUsername;
        }

        private void EditPassword(BaseContext cntx, Users query, MenuChoice usersMenuChoice)
        {
            Console.Clear();
            Console.WriteLine($"Current password: {cntx.Users.Where(u => u.Username == usersMenuChoice.choice).FirstOrDefault().Password}");
            string newPassword = Core.GetInput("New password");
            Console.WriteLine("In process . . .");
            query.Password = newPassword;
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
