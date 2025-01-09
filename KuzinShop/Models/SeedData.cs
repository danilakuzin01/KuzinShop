using Microsoft.AspNetCore.Identity;

namespace KuzinShop.Models
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            var roleNames = new[] { "Admin", "User", "Manager" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
