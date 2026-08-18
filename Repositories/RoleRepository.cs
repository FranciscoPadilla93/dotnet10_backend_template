using LUPA.Api.Data;
using LUPA.Api.Entities;
using LUPA.Api.Repositories.Base;
using LUPA.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LUPA.Api.Repositories;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Role?> GetByCodeAsync(string code)
    {
        return await Context.Roles
            .FirstOrDefaultAsync(x => x.Code == code && !x.IsDeleted);
    }

    public async Task<List<string>> GetPermissionCodesAsync(int roleId)
    {
        return await Context.RolePermissions
            .Where(x => x.RoleId == roleId)
            .Select(x => x.Permission.Code)
            .ToListAsync();
    }

    public async Task SetPermissionsAsync(int roleId, IEnumerable<int> permissionIds)
    {
        var currentRolePermissions = await Context.RolePermissions
            .Where(x => x.RoleId == roleId)
            .ToListAsync();

        Context.RolePermissions.RemoveRange(currentRolePermissions);

        foreach (var permissionId in permissionIds.Distinct())
        {
            Context.RolePermissions.Add(new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            });
        }

        await Context.SaveChangesAsync();
    }

    protected override IQueryable<Role> ApplySearch(IQueryable<Role> query, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return query;
        }

        return query.Where(x =>
            x.Code.Contains(search) ||
            x.Name.Contains(search) ||
            (x.Description != null && x.Description.Contains(search)));
    }

    protected override IQueryable<Role> ApplySort(IQueryable<Role> query, string? sortBy, bool descending)
    {
        return sortBy?.ToLower() switch
        {
            "code" => descending
                ? query.OrderByDescending(x => x.Code)
                : query.OrderBy(x => x.Code),

            "name" => descending
                ? query.OrderByDescending(x => x.Name)
                : query.OrderBy(x => x.Name),

            _ => query.OrderBy(x => x.Id)
        };
    }
}