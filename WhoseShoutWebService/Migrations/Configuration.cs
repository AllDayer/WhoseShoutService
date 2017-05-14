namespace WhoseShoutWebService.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WhoseShoutFormsPrism.Models;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<WhoseShoutWebService.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WhoseShoutWebService.Models.ApplicationDbContext context)
        {
            var shout = new Shout
            {
                ID = new Guid("45def82e-2b68-47e1-98c6-7d34906b46f1"),
                ShoutGroupID = new Guid("bf3641d1-a384-494d-a957-18f2aa42170c"),
                ShoutUserID = new Guid("d9c91004-3994-4bb4-a703-267904985126"),
                Cost = 9.0f,
                PurchaseTimeUtc = DateTime.UtcNow
            };

            var user1 = new ShoutUser
            {
                ID = new Guid("d9c91004-3994-4bb4-a703-267904985126"),
                UserName = "Tristan",
                FacebookID = "11111111111111111",
                ShoutGroups = new List<ShoutGroup>(),
                Shouts = new List<Shout>()
            };
            var user2 = new ShoutUser
            {
                ID = new Guid("c9c9f88b-853b-46e5-a70a-fad212fab6b0"),
                UserName = "Norman",
                TwitterID = "12519262411111111111",
                ShoutGroups = new List<ShoutGroup>(),
                Shouts = new List<Shout>()
            };
            var user3 = new ShoutUser
            {
                ID = new Guid("840a9916-ca86-4575-9025-6adb2abfa389"),
                UserName = "Elspeth",
                FacebookID = "11111111111111112",
                ShoutGroups = new List<ShoutGroup>(),
                Shouts = new List<Shout>()
            };

            var shoutGroup = new ShoutGroup
            {
                ID = new Guid("bf3641d1-a384-494d-a957-18f2aa42170c"),
                Category = "Coffee",
                Name = "CoffeeTime",
                ShoutUsers = new List<ShoutUser>(),
                Shouts = new List<Shout>()
            };

            var shoutGroup2 = new ShoutGroup
            {
                ID = new Guid("9befd37c-62c6-4fcf-9d77-8945c7964e7b"),
                Category = "Beer",
                Name = "Beeeeers",
                ShoutUsers = new List<ShoutUser>(),
                Shouts = new List<Shout>()
            };

            shout.ShoutUser = user1;
            shout.ShoutGroup = shoutGroup;

            user1.ShoutGroups.Add(shoutGroup);
            user2.ShoutGroups.Add(shoutGroup);

            user1.Shouts.Add(shout);

            shoutGroup.ShoutUsers.Add(user1);
            shoutGroup.ShoutUsers.Add(user2);
            shoutGroup.Shouts.Add(shout);

            shoutGroup2.ShoutUsers.Add(user3);

            context.Shouts.AddOrUpdate(p => p.ID, shout);
            context.ShoutUsers.AddOrUpdate(p => p.ID, user1, user2, user3);
            context.ShoutGroups.AddOrUpdate(p => p.ID, shoutGroup, shoutGroup2);
        }
    }
}
