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
        public DbSet<ChatOptions> ChatOptions { get; set; }
        public DbSet<Keys> Keys { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<ConferenceOptions> ConferenceOptions { get; set; }
        public DbSet<AdminOptions> AdminOptions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Использование Fluent API
            modelBuilder.Entity<Users>().Property(p => p.Username).IsRequired();   //Значение столбца не может быть равно null

            modelBuilder.Entity<Users>().Property(p => p.Username).HasMaxLength(50);

            //modelBuilder.Entity<Users>().ToTable("Users"); //все обьекты Assort будут храниться в таблице Ассортимент
            //modelBuilder.Ignore<Users>();//Если по какой-то сущности нам не надо создавать таблицу, 
            // то мы можем ее проигнорировать с помощью метода Ignore():
            base.OnModelCreating(modelBuilder);
        }
    }
}
