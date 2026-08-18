using System.Linq.Expressions;
using LUPA.Api.Common;
using LUPA.Api.Data;
using LUPA.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace LUPA.Api.Repositories.Base;

/// <summary>
/// Implementación genérica de IBaseRepository sobre EF Core.
/// Los repositorios concretos heredan de esta clase y sobrescriben
/// ApplySearch/ApplySort para definir en qué columnas buscar/ordenar.
/// </summary>
public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly ApplicationDbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    public BaseRepository(ApplicationDbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        return await BaseQuery().FirstOrDefaultAsync(x => x.Id == id);
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await BaseQuery().AnyAsync(predicate);
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        DbSet.Add(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;

        DbSet.Update(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        DbSet.Update(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task<PagedResult<TEntity>> GetPagedAsync(PaginationRequest request)
    {
        IQueryable<TEntity> query = BaseQuery();

        query = ApplySearch(query, request.Search);

        var totalRecords = await query.CountAsync();

        query = ApplySort(query, request.SortBy, request.Descending);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return new PagedResult<TEntity>
        {
            Items = items,
            TotalRecords = totalRecords
        };
    }

    /// <summary>
    /// Sobrescribe en el repositorio concreto para definir en qué columnas de texto
    /// buscar. Por defecto no filtra nada (search se ignora) para no romper entidades
    /// que todavía no lo implementen.
    /// </summary>
    protected virtual IQueryable<TEntity> ApplySearch(IQueryable<TEntity> query, string? search)
    {
        return query;
    }

    /// <summary>
    /// Sobrescribe en el repositorio concreto para mapear el string SortBy a una
    /// columna real. Por defecto ordena por Id.
    /// </summary>
    protected virtual IQueryable<TEntity> ApplySort(IQueryable<TEntity> query, string? sortBy, bool descending)
    {
        return descending
            ? query.OrderByDescending(x => x.Id)
            : query.OrderBy(x => x.Id);
    }

    /// <summary>
    /// Query base usada por GetByIdAsync/ExistsAsync/GetPagedAsync. Sobrescribe en el
    /// repositorio concreto para agregar .Include() cuando la entidad necesite cargar
    /// relaciones (ej. Report necesita sus Parameters siempre cargados).
    /// </summary>
    protected virtual IQueryable<TEntity> BaseQuery()
    {
        return DbSet.Where(x => !x.IsDeleted);
    }
}