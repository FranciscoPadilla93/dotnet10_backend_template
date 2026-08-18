using LUPA.Api.Data;

namespace LUPA.Api.Data.Seeders.Contracts;

public interface ISeeder
{
    Task SeedAsync(ApplicationDbContext context);
}