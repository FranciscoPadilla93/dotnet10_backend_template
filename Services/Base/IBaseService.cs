using LUPA.Api.Common;
using LUPA.Api.Responses;

namespace LUPA.Api.Services.Base;

public interface IBaseService<TResponse, TCreateRequest, TUpdateRequest>
{
    Task<PagedResponse<TResponse>> GetPagedAsync(PaginationRequest request);

    Task<TResponse> GetByIdAsync(int id);

    Task<TResponse> CreateAsync(TCreateRequest request);

    Task<TResponse> UpdateAsync(int id, TUpdateRequest request);

    Task DeleteAsync(int id);
}