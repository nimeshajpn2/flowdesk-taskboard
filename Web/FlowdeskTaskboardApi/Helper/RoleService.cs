using FlowdeskTaskboardApi.Shared;
using Microsoft.AspNetCore.Identity;

namespace FlowdeskTaskboardApi.Helper
{
    public class RoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        //Add User Roles
        public async Task SeedRolesAsync()
        {
            foreach (var role in Roles.AllRoles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
