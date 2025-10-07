namespace Dotnet.Template.Server.API.Responses;

public enum ErrorCode
{
    InternalServerError = 0,
    ValidationFailure = 1,
    Unauthorized = 2,
    ForbiddenAccess = 3,
    BadRequest = 4,
}
