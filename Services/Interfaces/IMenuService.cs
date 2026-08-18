using LUPA.Api.Requests;
using LUPA.Api.Responses;
using LUPA.Api.Services.Base;

namespace LUPA.Api.Services.Interfaces;

public interface IMenuService : IBaseService<MenuResponse, CreateMenuRequest, UpdateMenuRequest>
{
}