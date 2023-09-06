using Microsoft.AspNetCore.Identity;

namespace MiMatos.Services
{
    public class SeedUserRoles : ISeedUserRoles
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedUserRoles(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void SeedRoles()
        {
            if (!_roleManager.RoleExistsAsync("Convidado").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Convidado";
                role.NormalizedName = "CONVIDADO";
                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
            }
            if (!_roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                role.NormalizedName = "ADMIN";
                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
            }
        }

        public IdentityResult SeedUsers(IdentityUser user, string password)
        {
            if(user.UserName == "Michele")
            {
                IdentityResult result = _userManager.CreateAsync(user, password).Result;

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "Admin").Wait();
                }

                return result;
            }
            else
            {
                IdentityResult result = _userManager.CreateAsync(user, password).Result;

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "Convidado").Wait();
                }

                return result;
            }
        }
    }
}
