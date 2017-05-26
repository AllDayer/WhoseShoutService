namespace WhoseShoutWebService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class trackcost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShoutGroups", "TrackCost", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShoutGroups", "TrackCost");
        }
    }
}
