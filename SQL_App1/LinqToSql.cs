using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQL_App1
{
    public class LinqToSql : DataContext
    {
        public LinqToSql(string ConnString) : base(ConnString)
        {
            // this.dataSet = new DataSetLoader();
        }
        private static readonly string ConnString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        private static LinqTdsDataContext db = new LinqTdsDataContext(ConnString);
        

        public static DataSet ds = new DataSet();

        public static void Init()
        {
            //SelectUser("test");
//            cmd = new SqlCommand("SELECT * FROM accounts", connection);
//            adapter = new SqlDataAdapter(cmd);
//            connection.Open();
//            adapter.Fill(ds, "accounts");
//            cmd = new SqlCommand("SELECT * FROM Keys", connection);
//            adapter = new SqlDataAdapter(cmd);
//            adapter.Fill(ds, "Keys");
//            connection.Close();
//            Console.WriteLine("DataSet initialised!\r\nPress any key to continue. . .");
//            Console.ReadKey(true);
            LinqTdsDataContext context = new LinqTdsDataContext();
            //IEnumerable<accounts> accs = new

            /*var result = from accounts in ds.Tables["accounts"].AsEnumerable()
                select accounts;*/
            /*var result = from accounts in db.GetTable<accounts>()
                select accounts;*/

            Menu();
        }

        private static void Menu()
        {
            MenuChoice MainMenu = MenuManager.Menu(new string[] {"Create new chat", "Print \"accounts\" table", "Transaction", "Exit"}, "Messenger example usage Linq to Sql");
            while (MainMenu.choice != "Exit")
            {
                switch (MainMenu.choice)
                {
                    case "Create new chat":
                        NewChat();
                        break;
                    case "Print \"accounts\" table":
                        DispAccountsTable();
                        break;
                    case "Transaction":
                        Transaction();
                        break;
                    default:
                        break;
                }

                MenuManager.Menu(MainMenu);
            }
        }

        private static void DispAccountsTable()
        {
            Table<accounts> accs = db.GetTable<accounts>();
            foreach (var item in accs)
            {
                Console.WriteLine("{0,-15} \t{1,-15} \t{2,-15}", item.Id, item.username, item.password);

            }
        }

        private static void NewChat()
        {
            int creatorId = -1, member = -1;
            MenuChoice NewChatMenu = MenuManager.Menu(new string[] {"Select chat creator", "Select member", "Create chat", "Exit"}, $"New chat menu\r\nCreator: {creatorId}\r\nMember: {member}");
            while (NewChatMenu.choice != "Exit")
            {
                switch (NewChatMenu.choice_id)
                {
                    case 0:
                        creatorId = SelectUser("Please select chat creator");
                        break;
                    case 1:
                        member = SelectUser("Please select chat member");
                        break;
                    case 2:
                        CreateNewChat(creatorId, member);
                        break;
                }

                MenuManager.Menu(NewChatMenu, $"New chat menu\r\nCreator: {creatorId}\r\nMember: {member}");
            }
        }

        private static void CreateNewChat(int creatorId, int member)
        {
            LinqToSql lnq = new LinqToSql(ConnString);
            try
            {
                lnq.CreateChat(creatorId, member);
                MenuManager.Menu(new string[] {"Go back"}, "New chat was successfuly created!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey(true);
            }
        }

        [Function(Name = "CreateChat")]
        public int CreateChat(
            [Parameter(Name = "creatorId", DbType = "int")] int creatorId,
            [Parameter(Name = "member", DbType = "int")] int member)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), creatorId, member);
            return (int)result.ReturnValue;

        }

        private static void Transaction()
        {
            LinqTdsDataContext dt = new LinqTdsDataContext(ConnString);

            try
            {
                int sender = SelectUser("Please select message sender");
                int chatid = SelectChat("Please select chat to send message");
                string message = Core.GetInput("Message");

                db.GetTable<messages>().InsertOnSubmit(new messages() { sender = sender, chatId = chatid, message = message });
                db.GetTable<messages>().InsertOnSubmit(new messages() { sender = sender, chatId = chatid, message = message + " _additional" });
                
                db.SubmitChanges(ConflictMode.ContinueOnConflict);
                MenuManager.Menu(new string[] { "Go back" }, "Transaction successful!");
            }
            catch (ChangeConflictException)
            {
                db.ChangeConflicts.ResolveAll(RefreshMode.OverwriteCurrentValues);
                Console.WriteLine("Conflict");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                foreach (ObjectChangeConflict item in db.ChangeConflicts)
                {
                    item.Resolve(RefreshMode.KeepCurrentValues);
                }
            }
        }

        private static int SelectUser(string menuTitle)
        {
            Table<accounts> accs = db.GetTable<accounts>();
            List<string> usernamesList = new List<string>();
            foreach (var item in accs)
            {
                usernamesList.Add(item.username);
            }

            string[] usernames = usernamesList.ToArray();
            var menu = MenuManager.Menu(usernames, menuTitle);
            IQueryable<accounts> id = from a in db.accounts
                where a.username == menu.choice
                select a;
            return id.ToList().FirstOrDefault().Id;
        }

        private static int SelectChat(string menuTitle)
        {
            Table<chats> accs = db.GetTable<chats>();
            List<string> chatIdsList = new List<string>();
            foreach (var item in accs)
            {
                chatIdsList.Add(item.Id.ToString());
            }

            string[] chatIds = chatIdsList.ToArray();
            var menu = MenuManager.Menu(chatIds, menuTitle);
            IQueryable<accounts> id = from a in db.accounts
                where a.username == menu.choice
                select a;
            return id.ToList().FirstOrDefault().Id;
        }
    }

    public class MessageEntity
    {
        public int id = -1;
        public int sender;
        public int chatId;
        public string message;
    }
}
