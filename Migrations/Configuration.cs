namespace Portfolio.Migrations {
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Portfolio.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;

    internal sealed class Configuration : DbMigrationsConfiguration<Portfolio.Models.ApplicationDbContext> {
        public Configuration() {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Portfolio.Models.ApplicationDbContext context) {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!context.Users.Any(u => u.Email == "brandon@navicamls.net")) {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            else {
                roleManager.Create(new IdentityRole { Name = "User" });
            };
            //if (!context.Roles.Any(r => r.Name == "Admin")) {
            //    roleManager.Create(new IdentityRole { Name = "Admin" });
            //}

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "brandon@navicamls.net")) {
                userManager.Create(new ApplicationUser {
                    UserName = "brandon@navicamls.net",
                    Email = "brandon@navicamls.net",
                    FirstName = "Brandon",
                    LastName = "Payne",
                    DisplayName = "Brandon Payne",
                    EmailConfirmed = true,
                    IsLoggedIn = true,
                    CanPost = true
                },
                    "billybob");
            }

            var userId = userManager.FindByEmail("brandon@navicamls.net").Id;
            userManager.AddToRole(userId, "Admin");
        }
    }
}
