namespace WhoseShoutWebService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class avatar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShoutUsers", "AvatarUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShoutUsers", "AvatarUrl");
        }
    }
}
