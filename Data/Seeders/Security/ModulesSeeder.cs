using LUPA.Api.Data.Seeders.Contracts;
using LUPA.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace LUPA.Api.Data.Seeders.Security;

public class ModulesSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Modules.AnyAsync())
            return;

        var modules = new List<Module>
        {
            new Module
            {
                Code = "SECURITY",
                Name = "Seguridad",
                Description = "Administración de usuarios, roles y permisos",
                Icon = "shield",
                SortOrder = 1
            }
        };

        await context.Modules.AddRangeAsync(modules);
        await context.SaveChangesAsync();
    }
}