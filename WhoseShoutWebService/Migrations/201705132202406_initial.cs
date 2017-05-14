namespace WhoseShoutWebService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ShoutGroups",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(),
                        Category = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Shouts",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        ShoutGroupID = c.Guid(nullable: false),
                        ShoutUserID = c.Guid(nullable: false),
                        PurchaseTimeUtc = c.DateTime(nullable: false),
                        Cost = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ShoutGroups", t => t.ShoutGroupID, cascadeDelete: true)
                .ForeignKey("dbo.ShoutUsers", t => t.ShoutUserID, cascadeDelete: true)
                .Index(t => t.ShoutGroupID)
                .Index(t => t.ShoutUserID);
            
            CreateTable(
                "dbo.ShoutUsers",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UserName = c.String(),
                        FacebookID = c.String(),
                        TwitterID = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ShoutUserShoutGroups",
                c => new
                    {
                        ShoutUser_ID = c.Guid(nullable: false),
                        ShoutGroup_ID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ShoutUser_ID, t.ShoutGroup_ID })
                .ForeignKey("dbo.ShoutUsers", t => t.ShoutUser_ID, cascadeDelete: true)
                .ForeignKey("dbo.ShoutGroups", t => t.ShoutGroup_ID, cascadeDelete: true)
                .Index(t => t.ShoutUser_ID)
                .Index(t => t.ShoutGroup_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Shouts", "ShoutUserID", "dbo.ShoutUsers");
            DropForeignKey("dbo.ShoutUserShoutGroups", "ShoutGroup_ID", "dbo.ShoutGroups");
            DropForeignKey("dbo.ShoutUserShoutGroups", "ShoutUser_ID", "dbo.ShoutUsers");
            DropForeignKey("dbo.Shouts", "ShoutGroupID", "dbo.ShoutGroups");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.ShoutUserShoutGroups", new[] { "ShoutGroup_ID" });
            DropIndex("dbo.ShoutUserShoutGroups", new[] { "ShoutUser_ID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Shouts", new[] { "ShoutUserID" });
            DropIndex("dbo.Shouts", new[] { "ShoutGroupID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.ShoutUserShoutGroups");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ShoutUsers");
            DropTable("dbo.Shouts");
            DropTable("dbo.ShoutGroups");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
        }
    }
}
