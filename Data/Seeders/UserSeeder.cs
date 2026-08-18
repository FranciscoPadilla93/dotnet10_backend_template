using LUPA.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace LUPA.Api.Data.Seeders;

public class UserSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Users.AnyAsync(x => x.Username == "admin"))
            return;

        var role = await context.Roles
            .FirstAsync(x => x.Code == "SUPER_ADMIN");

        var user = new User
        {
            Username = "admin",
            Email = "admin@lupa.local",
            PasswordHash = "$2a$11$FRebQ2ErwscW/gvqnIj9QOd9bbdDhv9W/5mcJc7ozdk0Cnecsd5ee",
            FirstName = "Administrador",
            LastName = "Sistema",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        context.Users.Add(user);

        await context.SaveChangesAsync();

        context.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id
        });

        await context.SaveChangesAsync();
    }
}