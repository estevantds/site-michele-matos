using Microsoft.AspNetCore.Identity;

namespace MiMatos.Services
{
    public interface ISeedUserRoles
    {
        void SeedRoles();
        IdentityResult SeedUsers(IdentityUser user, string password);
    }
}
