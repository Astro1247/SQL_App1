namespace SQL_App1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addchatoptions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "Chats_Id", c => c.Int());
            CreateIndex("dbo.Messages", "Chats_Id");
            AddForeignKey("dbo.Messages", "Chats_Id", "dbo.Chats", "Id");
            DropColumn("dbo.Chats", "MessagesCount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Chats", "MessagesCount", c => c.Int(nullable: false));
            DropForeignKey("dbo.Messages", "Chats_Id", "dbo.Chats");
            DropIndex("dbo.Messages", new[] { "Chats_Id" });
            DropColumn("dbo.Messages", "Chats_Id");
        }
    }
}
