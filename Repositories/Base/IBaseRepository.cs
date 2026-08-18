using System.Linq.Expressions;
using LUPA.Api.Common;
using LUPA.Api.Entities;

namespace LUPA.Api.Repositories.Base;

/// <summary>
/// Operaciones CRUD comunes a cualquier entidad que herede de BaseEntity.
/// Los repositorios concretos (ej. IUserRepository) heredan de esta interfaz
/// y solo agregan lo que sea específico de esa entidad.
/// </summary>
public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    Task<PagedResult<TEntity>> GetPagedAsync(PaginationRequest request);

    Task<TEntity?> GetByIdAsync(int id);

    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

    Task AddAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// Soft delete: marca IsDeleted = true, no borra la fila físicamente.
    /// </summary>
    Task DeleteAsync(TEntity entity);
}