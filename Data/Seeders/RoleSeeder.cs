using LUPA.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace LUPA.Api.Data.Seeders;

public class RoleSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Roles.AnyAsync(x => x.Code == "SUPER_ADMIN"))
            return;

        context.Roles.Add(new Role
        {
            Code = "SUPER_ADMIN",
            Name = "Super Admin",
            Description = "Administrador del sistema",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        });

        await context.SaveChangesAsync();
    }
}