namespace SQL_App1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _14 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Chats", "ChatOptions_Id", "dbo.ChatOptions");
            DropIndex("dbo.Chats", new[] { "ChatOptions_Id" });
            AlterColumn("dbo.Chats", "ChatOptions_Id", c => c.Int());
            CreateIndex("dbo.Chats", "ChatOptions_Id");
            AddForeignKey("dbo.Chats", "ChatOptions_Id", "dbo.ChatOptions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Chats", "ChatOptions_Id", "dbo.ChatOptions");
            DropIndex("dbo.Chats", new[] { "ChatOptions_Id" });
            AlterColumn("dbo.Chats", "ChatOptions_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Chats", "ChatOptions_Id");
            AddForeignKey("dbo.Chats", "ChatOptions_Id", "dbo.ChatOptions", "Id", cascadeDelete: true);
        }
    }
}
