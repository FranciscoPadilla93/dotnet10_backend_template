using LUPA.Api.Data;
using LUPA.Api.Entities;
using LUPA.Api.Repositories.Base;
using LUPA.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LUPA.Api.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await Context.Users
            .FirstOrDefaultAsync(x => x.Username == username && !x.IsDeleted);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await Context.Users
            .FirstOrDefaultAsync(x => x.Email == email && !x.IsDeleted);
    }

    public async Task<List<string>> GetRolesAsync(int userId)
    {
        return await Context.UserRoles
            .Where(x => x.UserId == userId)
            .Select(x => x.Role.Name)
            .ToListAsync();
    }

    public async Task<List<string>> GetPermissionCodesAsync(int userId)
    {
        return await Context.UserRoles
            .Where(x => x.UserId == userId)
            .SelectMany(x => x.Role.RolePermissions)
            .Select(x => x.Permission.Code)
            .Distinct()
            .ToListAsync();
    }

    public async Task SetRolesAsync(int userId, IEnumerable<int> roleIds)
    {
        var currentUserRoles = await Context.UserRoles
            .Where(x => x.UserId == userId)
            .ToListAsync();

        Context.UserRoles.RemoveRange(currentUserRoles);

        foreach (var roleId in roleIds.Distinct())
        {
            Context.UserRoles.Add(new UserRole
            {
                UserId = userId,
                RoleId = roleId
            });
        }

        await Context.SaveChangesAsync();
    }

    protected override IQueryable<User> ApplySearch(IQueryable<User> query, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return query;
        }

        return query.Where(x =>
            x.Username.Contains(search) ||
            x.Email.Contains(search) ||
            x.FirstName.Contains(search) ||
            x.LastName.Contains(search));
    }

    protected override IQueryable<User> ApplySort(IQueryable<User> query, string? sortBy, bool descending)
    {
        return sortBy?.ToLower() switch
        {
            "username" => descending
                ? query.OrderByDescending(x => x.Username)
                : query.OrderBy(x => x.Username),

            "email" => descending
                ? query.OrderByDescending(x => x.Email)
                : query.OrderBy(x => x.Email),

            "firstname" => descending
                ? query.OrderByDescending(x => x.FirstName)
                : query.OrderBy(x => x.FirstName),

            "lastname" => descending
                ? query.OrderByDescending(x => x.LastName)
                : query.OrderBy(x => x.LastName),

            _ => query.OrderBy(x => x.Id)
        };
    }
}