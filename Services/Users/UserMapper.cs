using LUPA.Api.Entities;
using LUPA.Api.Responses;

namespace LUPA.Api.Services.Users;

public static class UserMapper
{
    public static UserResponse ToResponse(
        User user,
        IReadOnlyCollection<string> roles)
    {
        return new UserResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsActive = user.IsActive,
            LastLogin = user.LastLogin,
            Roles = roles
        };
    }
}
