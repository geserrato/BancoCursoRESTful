using Application.Enums;
using Identity.Model;
using Microsoft.AspNetCore.Identity;

namespace Identity.Seeds;

public class DefaultBasicUser
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        //Seed Default Admin User
        var defaultUser = new ApplicationUser
        {
            UserName = "userBasic",
            Email = "userBasic@mail.com",
            Name = "Pedro",
            Surname = "Vasquez",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "123Pa$word");
                await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
            }
        }
    }
}