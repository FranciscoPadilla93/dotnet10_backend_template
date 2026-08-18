using System.Net;

namespace LUPA.Api.Common.Exceptions;

public sealed class UnauthorizedException : AppException
{
    public UnauthorizedException(string message) : base(message, HttpStatusCode.Unauthorized)
    {
    }
}