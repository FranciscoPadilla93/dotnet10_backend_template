using LUPA.Api.Common;
using LUPA.Api.Common.Exceptions;
using System.Net;

namespace LUPA.Api.Middlewares;

public sealed class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            var statusCode = exception is AppException appException ? (int)appException.StatusCode : (int)HttpStatusCode.InternalServerError;

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = BuildResponse(exception, context);

            await context.Response.WriteAsJsonAsync(response);
        }
    }

    private static ApiResponse<object> BuildResponse(Exception exception, HttpContext context)
    {
        return new ApiResponse<object>
        {
            Success = false,
            Message = exception.Message,
            Errors = [exception.Message],
            TraceId = context.TraceIdentifier
        };
    }
}
