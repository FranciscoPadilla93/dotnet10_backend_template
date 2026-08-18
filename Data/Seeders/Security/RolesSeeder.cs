using LUPA.Api.Data.Seeders.Contracts;
using LUPA.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace LUPA.Api.Data.Seeders.Security;

public class RolesSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Roles.AnyAsync())
            return;

        var roles = new List<Role>
        {
            new Role
            {
                Code = "ADMIN",
                Name = "Administrador",
                Description = "Acceso total al sistema"
            }
        };

        await context.Roles.AddRangeAsync(roles);
        await context.SaveChangesAsync();
    }
}