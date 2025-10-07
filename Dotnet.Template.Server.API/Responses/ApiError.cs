namespace Dotnet.Template.Server.API.Responses;

/// <summary>
/// All exceptions should be caught in our middleware and converted to this type.
/// This provides a uniform way to handle exceptions in the UI.
/// </summary>
public class ApiError
{
    public ApiError(string message, ErrorCode code, Guid errorId)
    {
        Message = message;
        Code = code.ToString();
        ErrorId = errorId;
    }

    /// <summary>
    /// User friendly message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// A flag indicating the type of error for UI handling.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// A guid that is tied to the logged record.
    /// </summary>
    public Guid ErrorId { get; set; }
}
