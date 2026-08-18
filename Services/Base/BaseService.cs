using LUPA.Api.Common;
using LUPA.Api.Common.Audit;
using LUPA.Api.Common.Exceptions;
using LUPA.Api.Entities;
using LUPA.Api.Repositories.Base;
using LUPA.Api.Responses;
using LUPA.Api.Services.Audit;

namespace LUPA.Api.Services.Base;

public abstract class BaseService<TEntity, TResponse, TCreateRequest, TUpdateRequest>
    : IBaseService<TResponse, TCreateRequest, TUpdateRequest>
    where TEntity : BaseEntity
{
    protected readonly IBaseRepository<TEntity> Repository;
    protected readonly IAuditLogService AuditLog;

    protected BaseService(IBaseRepository<TEntity> repository, IAuditLogService auditLogService)
    {
        Repository = repository;
        AuditLog = auditLogService;
    }

    protected virtual string NotFoundMessage => "Registro no encontrado.";

    /// <summary>Nombre de la entidad tal como aparece en el AuditLog. Por defecto, el nombre de la clase C#.</summary>
    protected virtual string EntityName => typeof(TEntity).Name;

    protected abstract Task<TResponse> MapToResponseAsync(TEntity entity);

    protected abstract Task<TEntity> MapToEntityAsync(TCreateRequest request);

    protected abstract Task ApplyUpdateAsync(TEntity entity, TUpdateRequest request);

    protected virtual Task AfterCreateAsync(TEntity entity, TCreateRequest request) => Task.CompletedTask;

    protected virtual Task AfterUpdateAsync(TEntity entity, TUpdateRequest request) => Task.CompletedTask;

    public virtual async Task<PagedResponse<TResponse>> GetPagedAsync(PaginationRequest request)
    {
        var result = await Repository.GetPagedAsync(request);

        var items = new List<TResponse>();

        foreach (var entity in result.Items)
        {
            items.Add(await MapToResponseAsync(entity));
        }

        return new PagedResponse<TResponse>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalRecords = result.TotalRecords
        };
    }

    public virtual async Task<TResponse> GetByIdAsync(int id)
    {
        var entity = await Repository.GetByIdAsync(id)
            ?? throw new NotFoundException(NotFoundMessage);

        return await MapToResponseAsync(entity);
    }

    public virtual async Task<TResponse> CreateAsync(TCreateRequest request)
    {
        var entity = await MapToEntityAsync(request);

        await Repository.AddAsync(entity);

        await AfterCreateAsync(entity, request);

        await AuditLog.LogAsync(
            "CREATE", EntityName, entity.Id.ToString(),
            beforeJson: null,
            afterJson: AuditSerializer.Serialize(entity));

        return await MapToResponseAsync(entity);
    }

    public virtual async Task<TResponse> UpdateAsync(int id, TUpdateRequest request)
    {
        var entity = await Repository.GetByIdAsync(id)
            ?? throw new NotFoundException(NotFoundMessage);

        // Snapshot ANTES de mutar: como ApplyUpdateAsync modifica el mismo objeto en memoria,
        // hay que serializar aquí, no después, o "antes" y "después" quedarían idénticos.
        string? beforeJson = AuditSerializer.Serialize(entity);

        await ApplyUpdateAsync(entity, request);

        await Repository.UpdateAsync(entity);

        await AfterUpdateAsync(entity, request);

        string? afterJson = AuditSerializer.Serialize(entity);

        await AuditLog.LogAsync("UPDATE", EntityName, id.ToString(), beforeJson, afterJson);

        return await MapToResponseAsync(entity);
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await Repository.GetByIdAsync(id)
            ?? throw new NotFoundException(NotFoundMessage);

        string? beforeJson = AuditSerializer.Serialize(entity);

        await Repository.DeleteAsync(entity);

        await AuditLog.LogAsync("DELETE", EntityName, id.ToString(), beforeJson, afterJson: null);
    }
}