using Microsoft.AspNetCore.Identity;
using SimplexTask.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplexTask.DataAccess
{
    public static class AdminSeed
    {
        public async static Task Seed(DataContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            var MainRole = "Admin";
            context.Database.EnsureCreated();

            if (!roleManager.Roles.Any(x => x.Name == "Admin"))
                await roleManager.CreateAsync(new IdentityRole(MainRole));

            var user = await userManager.FindByEmailAsync("admin@app.com");
            if (user is null)
            {
                var admin = new User
                {
                    Email = "admin@app.com",
                    UserName = "admin@app.com",
                    Fullname = "Admin",
                };
                await userManager.CreateAsync(admin, "abcde12345");
                await userManager.AddToRoleAsync(admin, MainRole);

                

            }
        }
    }
}
