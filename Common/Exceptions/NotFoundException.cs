using System.Net;

namespace LUPA.Api.Common.Exceptions;

public sealed class NotFoundException : AppException
{
    public NotFoundException(string message) : base(message, HttpStatusCode.NotFound)
    {
    }
}