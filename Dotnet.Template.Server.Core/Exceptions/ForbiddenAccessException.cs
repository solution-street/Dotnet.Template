namespace Dotnet.Template.Server.Core.Exceptions;

/// <summary>
/// An exception that should be used when a user tries to access a resource they are not allowed to.
/// This exception type is handled in our exception handling middleware and the appropriate response is returned.
/// </summary>
public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException()
        : base() { }

    public ForbiddenAccessException(string message)
        : base(message) { }

    public ForbiddenAccessException(string message, Exception innerException)
        : base(message, innerException) { }
}
