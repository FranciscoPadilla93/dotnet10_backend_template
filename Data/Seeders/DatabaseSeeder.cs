using Microsoft.EntityFrameworkCore;

namespace LUPA.Api.Data.Seeders;

public class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await RoleSeeder.SeedAsync(context);
        await UserSeeder.SeedAsync(context);
    }
}
