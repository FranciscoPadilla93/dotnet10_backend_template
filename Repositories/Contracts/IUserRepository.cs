using LUPA.Api.Entities;
using LUPA.Api.Repositories.Base;

namespace LUPA.Api.Repositories.Contracts;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);

    Task<User?> GetByEmailAsync(string email);

    Task<List<string>> GetRolesAsync(int userId);

    Task<List<string>> GetPermissionCodesAsync(int userId);

    Task SetRolesAsync(int userId, IEnumerable<int> roleIds);
}