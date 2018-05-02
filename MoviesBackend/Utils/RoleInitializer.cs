using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MoviesBackend.Utils
{
  public class RoleInitializer
  {
    public static async Task InitializeAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
      const string adminEmail = "test@test.com";
      const string adminPassword = "Admin&Admin1";
      if (await roleManager.FindByNameAsync(Roles.ADMIN) == null)
      {
        await roleManager.CreateAsync(new IdentityRole(Roles.ADMIN));
      }
      if (await roleManager.FindByNameAsync(Roles.USER) == null)
      {
        await roleManager.CreateAsync(new IdentityRole(Roles.USER));
      }
      if (await userManager.FindByNameAsync(adminEmail) == null)
      {
        IdentityUser admin = new IdentityUser
        {
          Email = adminEmail,
          UserName = adminEmail
        };
        IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
        if (result.Succeeded)
        {
          await userManager.AddToRoleAsync(admin, Roles.ADMIN);
        }
      }
    }
  }
}

