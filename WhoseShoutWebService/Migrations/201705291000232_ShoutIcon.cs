namespace WhoseShoutWebService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShoutIcon : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShoutGroupIcons",
                c => new
                    {
                        ShoutGroupID = c.Guid(nullable: false),
                        ShoutIconIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ShoutGroupID)
                .ForeignKey("dbo.ShoutGroups", t => t.ShoutGroupID)
                .Index(t => t.ShoutGroupID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShoutGroupIcons", "ShoutGroupID", "dbo.ShoutGroups");
            DropIndex("dbo.ShoutGroupIcons", new[] { "ShoutGroupID" });
            DropTable("dbo.ShoutGroupIcons");
        }
    }
}
