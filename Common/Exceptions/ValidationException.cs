using System.Net;

namespace LUPA.Api.Common.Exceptions;

public sealed class ValidationException : AppException
{
    public ValidationException(string message) : base(message, HttpStatusCode.BadRequest)
    {
    }
}