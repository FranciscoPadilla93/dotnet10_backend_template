using LUPA.Api.Common.Excel;
using LUPA.Api.Requests;
using LUPA.Api.Responses;
using LUPA.Api.Services.Base;

namespace LUPA.Api.Services.Interfaces;

public interface IUserService : IBaseService<UserResponse, CreateUserRequest, UpdateUserRequest>
{
    Task ActivateAsync(int id, bool isActive);
    Task ChangePasswordAsync(int id, ChangePasswordRequest request);
    Task<ExcelImportResult> ImportFromExcelAsync(Stream fileStream);
}