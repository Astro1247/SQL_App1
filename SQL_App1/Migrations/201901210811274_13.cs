namespace SQL_App1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChatOptions", "Period", c => c.Int());
            AddColumn("dbo.ChatOptions", "NotifyOnMemberJoin", c => c.Boolean());
            AddColumn("dbo.ChatOptions", "MaxAdminCount", c => c.Int());
            AddColumn("dbo.ChatOptions", "AdminAllRights", c => c.Boolean());
            AddColumn("dbo.ChatOptions", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChatOptions", "Discriminator");
            DropColumn("dbo.ChatOptions", "AdminAllRights");
            DropColumn("dbo.ChatOptions", "MaxAdminCount");
            DropColumn("dbo.ChatOptions", "NotifyOnMemberJoin");
            DropColumn("dbo.ChatOptions", "Period");
        }
    }
}
