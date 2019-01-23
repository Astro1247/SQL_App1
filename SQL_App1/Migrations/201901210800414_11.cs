namespace SQL_App1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatOptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AnnounceOnly = c.Boolean(nullable: false),
                        InviteOnly = c.Boolean(nullable: false),
                        IsHidden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ChatOptions", new[] { "Id" });
            DropTable("dbo.ChatOptions");
        }
    }
}
