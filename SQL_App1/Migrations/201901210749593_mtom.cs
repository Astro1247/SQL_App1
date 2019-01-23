namespace SQL_App1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mtom : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Keys", "User_Id", "dbo.Users");
            DropIndex("dbo.Keys", new[] { "User_Id" });
            CreateTable(
                "dbo.UsersKeys",
                c => new
                    {
                        Users_Id = c.Int(nullable: false),
                        Keys_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Users_Id, t.Keys_Id })
                .ForeignKey("dbo.Users", t => t.Users_Id, cascadeDelete: true)
                .ForeignKey("dbo.Keys", t => t.Keys_Id, cascadeDelete: true)
                .Index(t => t.Users_Id)
                .Index(t => t.Keys_Id);
            
            AlterColumn("dbo.Users", "Username", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Keys", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Keys", "User_Id", c => c.Int());
            DropForeignKey("dbo.UsersKeys", "Keys_Id", "dbo.Keys");
            DropForeignKey("dbo.UsersKeys", "Users_Id", "dbo.Users");
            DropIndex("dbo.UsersKeys", new[] { "Keys_Id" });
            DropIndex("dbo.UsersKeys", new[] { "Users_Id" });
            AlterColumn("dbo.Users", "Username", c => c.String(nullable: false, maxLength: 255));
            DropTable("dbo.UsersKeys");
            CreateIndex("dbo.Keys", "User_Id");
            AddForeignKey("dbo.Keys", "User_Id", "dbo.Users", "Id");
        }
    }
}
