using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchAPI.Data.Static;
using TwitchAPI.Models.AppUsers;

namespace eTickets.Data
{
    public class AppUserInitializer
    {
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(ApplicationRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(ApplicationRoles.Admin));
                if (!await roleManager.RoleExistsAsync(ApplicationRoles.TwitchValidatedUser))
                    await roleManager.CreateAsync(new IdentityRole(ApplicationRoles.TwitchValidatedUser));
                if (!await roleManager.RoleExistsAsync(ApplicationRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(ApplicationRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                string adminUserEmail = "admin@twitchapi.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        TwitchUserId = 123,
                        UserName = "admin-user",
                        Email = adminUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "12345678Qq@");
                    await userManager.AddToRoleAsync(newAdminUser, ApplicationRoles.Admin);
                }

                string appUserEmail = "user@twitchapi.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new ApplicationUser()
                    {
                        TwitchUserId = 145,
                        UserName = "app-user",
                        Email = appUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAppUser, "12345678Qq@");
                    await userManager.AddToRoleAsync(newAppUser, ApplicationRoles.User);
                }
            }
        }
    }
}
