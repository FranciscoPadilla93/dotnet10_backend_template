using LUPA.Api.Common.Email;

namespace LUPA.Api.Services.Email;

public interface IEmailService
{
    /// <summary>
    /// Encola el correo para envío en segundo plano. Regresa de inmediato: NO espera
    /// a que el servidor SMTP responda, para no bloquear la petición HTTP del usuario.
    /// </summary>
    ValueTask QueueEmailAsync(EmailMessage message);
}