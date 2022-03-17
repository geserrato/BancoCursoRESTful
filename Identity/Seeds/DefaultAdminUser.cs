using Application.Enums;
using Identity.Model;
using Microsoft.AspNetCore.Identity;

namespace Identity.Seeds;

public static  class DefaultAdminUser
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        //Seed Default Admin User
        var defaultUser = new ApplicationUser
        {
            UserName = "userAdmin",
            Email = "userAdmin@mail.com",
            Name = "Gerardo",
            Surname = "Estrella",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(defaultUser.Email);
            if(user == null)
            {
                await userManager.CreateAsync(defaultUser, "123Pa$word");
                await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
            }
        }
    }
}