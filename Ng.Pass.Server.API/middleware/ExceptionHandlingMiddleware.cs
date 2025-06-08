using System.Net;
using System.Text.Json;
using FluentValidation;
using Ng.Pass.Server.API.Responses;
using Ng.Pass.Server.Core.Exceptions;

namespace Ng.Pass.Server.API.Middleware;

/// <summary>
/// Middleware to handle all uncaught exceptions. If you are unable to gracefully handle an exception, then we want to let it bubble up to
/// this middleware to ensure uniform logging and response handling.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            // Call the next middleware in the pipeline
            await _next(context);
        }
        catch (Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            Guid errorId = Guid.NewGuid();
            ErrorCode errorCode;
            string[] message;
            switch (ex)
            {
                case ValidationException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    message = e?.Errors?.Select(s => s.ErrorMessage)?.ToArray() ?? ["Internal Exception Occured"];
                    errorCode = ErrorCode.ValidationFailure;
                    break;

                case UnauthorizedAccessException e:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    message = ["Unauthorized Access"];
                    errorCode = ErrorCode.Unauthorized;
                    break;

                case ForbiddenAccessException e:
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    message = ["Forbidden Access"];
                    errorCode = ErrorCode.ForbiddenAccess;
                    break;

                case BadRequestException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    message = ["Bad Request"];
                    errorCode = ErrorCode.BadRequest;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    message = ["Internal Exception Occurred"];
                    errorCode = ErrorCode.InternalServerError;
                    break;
            }

            var messages = string.Join(", ", message);

            _logger.LogError(
                exception: ex,
                message: "Exception occurred with ID {ErrorId}. Code: {ErrorCode}. Message: {ErrorMessage}",
                errorId,
                errorCode,
                messages
            );
            await response.WriteAsync(JsonSerializer.Serialize(new ApiError(messages, errorCode, errorId)));
        }
    }
}
