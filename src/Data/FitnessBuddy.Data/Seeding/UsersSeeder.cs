namespace FitnessBuddy.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var password = "123456";

            if (!dbContext.Users.Any(x => x.Email == "user@user.com"))
            {
                var user = new ApplicationUser
                {
                    Email = "user@user.com",
                    UserName = "user@user.com",
                };

                await userManager.CreateAsync(user, password);
            }

            if (dbContext.Roles.Any() == false)
            {
                return;
            }

            if (dbContext.Users.Any(u => u.Email == "admin@admin.com"))
            {
                return;
            }

            var admin = new ApplicationUser
            {
                Email = "admin@admin.com",
                UserName = "admin@admin.com",
            };

            admin.Roles.Add(new IdentityUserRole<string>()
            {
                RoleId = dbContext.Roles
                    .FirstOrDefault(r => r.Name == GlobalConstants.AdministratorRoleName)?.Id,
            });

            await userManager.CreateAsync(admin, password);
        }
    }
}
