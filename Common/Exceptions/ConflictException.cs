using System.Net;

namespace LUPA.Api.Common.Exceptions;

public sealed class ConflictException : AppException
{
    public ConflictException(string message) : base(message, HttpStatusCode.Conflict)
    {
    }
}