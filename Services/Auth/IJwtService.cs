using LUPA.Api.Entities;

namespace LUPA.Api.Services.Auth;

public interface IJwtService
{
    string GenerateAccessToken(User user, IEnumerable<string> roles, IEnumerable<string> permissions);

    string GenerateRefreshToken();

    /// <summary>
    /// Genera el hash (SHA-256) de un refresh token para almacenarlo o buscarlo en base de datos.
    /// El valor en texto plano solo debe existir en la respuesta al cliente, nunca persistido.
    /// </summary>
    string HashToken(string token);
}