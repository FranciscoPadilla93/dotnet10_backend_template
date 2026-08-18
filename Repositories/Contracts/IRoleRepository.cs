using LUPA.Api.Entities;
using LUPA.Api.Repositories.Base;

namespace LUPA.Api.Repositories.Contracts;

public interface IRoleRepository : IBaseRepository<Role>
{
    Task<Role?> GetByCodeAsync(string code);

    Task<List<string>> GetPermissionCodesAsync(int roleId);

    Task SetPermissionsAsync(int roleId, IEnumerable<int> permissionIds);
}