using LUPA.Api.Common;
using LUPA.Api.Common.Email;
using LUPA.Api.Common.Exceptions;
using LUPA.Api.Services.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LUPA.Api.Controllers;

[ApiController]
[Route("api/system")]
public class SystemController : ControllerBase
{
    [HttpGet("ping")]
    public async Task<IActionResult> Ping()
    {
        await Task.CompletedTask;

        var response = new ApiResponse<string>
        {
            Success = true,
            Message = "API disponible.",
            Data = "LUPA API Online"
        };

        return Ok(response);
    }

    [HttpGet("error")]
    public IActionResult Error()
    {
        throw new NotFoundException("Esta es una excepción de prueba.");
    }

    // [Authorize] a propósito (sin permiso específico): es solo un endpoint de diagnóstico
    // para probar la configuración SMTP. Requiere estar logueado para que nadie use tu API
    // como relay abierto de spam. Considera quitarlo del todo cuando cierres el template.
    [Authorize]
    [HttpPost("test-email")]
    public async Task<IActionResult> TestEmail(
        [FromServices] IEmailService emailService,
        [FromQuery] string to)
    {
        await emailService.QueueEmailAsync(new EmailMessage
        {
            To = to,
            Subject = "Prueba de LUPA",
            Body = "<p>Si ves esto, tu configuración SMTP funciona correctamente.</p>",
            IsHtml = true
        });

        return Ok(new ApiResponse<string>
        {
            Success = true,
            Message = "Correo encolado. Revisa la bandeja de entrada en unos segundos."
        });
    }
}