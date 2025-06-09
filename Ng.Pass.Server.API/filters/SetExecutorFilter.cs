using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Filters;
using Ng.Pass.Server.Core.Models;
using Ng.Pass.Server.Services.Shared.Services;

namespace Ng.Pass.Server.API.filters;

/// <summary>
/// A filter that sets executor property for all database context-related operations.
/// This ensures we are properly tracking who, if authenticated, is making the request.
/// </summary>
public class SetExecutorFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var executorService = context.HttpContext.RequestServices.GetService<IExecutorService>();
        if (executorService is null)
        {
            throw new InvalidOperationException("Executor service could not be retrieved from executing context.");
        }

        // Get the executor and set it on the request object.
        var executor = await executorService.TryGet(context.HttpContext.User);

        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is BaseRequest request)
            {
                request.Executor = executor;

                break;
            }
        }

        await next();
    }
}
