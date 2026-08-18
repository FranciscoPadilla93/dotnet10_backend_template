using LUPA.Api.Entities;
using LUPA.Api.Responses;

namespace LUPA.Api.Services.Modules;

public static class ModuleMapper
{
    public static ModuleResponse ToResponse(Module module)
    {
        return new ModuleResponse
        {
            Id = module.Id,
            Code = module.Code,
            Name = module.Name,
            Description = module.Description,
            Icon = module.Icon,
            SortOrder = module.SortOrder,
            IsActive = module.IsActive
        };
    }
}