namespace WhoseShoutWebService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShoutUsers", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShoutUsers", "Email");
        }
    }
}
