using System.Net;

namespace LUPA.Api.Common.Exceptions;

public sealed class ForbiddenException : AppException
{
    public ForbiddenException(string message) : base(message, HttpStatusCode.Forbidden)
    {
    }
}