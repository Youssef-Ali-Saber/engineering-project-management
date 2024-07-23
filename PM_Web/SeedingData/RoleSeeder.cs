using Microsoft.AspNetCore.Identity;

namespace PM.SeedingData
{
    public class RoleSeeder
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleSeeder(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task SeedRolesAsync()
        {

            var roles = new List<string> { "Cordinator", "TeamMember", "TeamManager", "Contractor" , "ContractorTeamMember" };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role)
                    {
                        NormalizedName = role.ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    });
                }
            }
        }
    }

}
