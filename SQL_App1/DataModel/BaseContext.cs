using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL_App1.DataModel
{
    class BaseContext : DbContext
    {
        public BaseContext() : base("EfConnString")
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Chats> Chats { get; set; }
        public DbSet<Keys> Keys { get; set; }
        public DbSet<Messages> Messages { get; set; }
    }
}
