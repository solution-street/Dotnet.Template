using System;
using Microsoft.AspNetCore.Mvc;
using Dotnet.Template.Server.API.filters;
using Dotnet.Template.Server.Core.Models;
using Dotnet.Template.Server.Services.Secrets.Models;
using Dotnet.Template.Server.Services.Shared.Services;

namespace Dotnet.Template.Server.API.Controllers;

/// <summary>
/// Base controller that all application controllers inherit from. This ensures proper access to common services.
/// </summary>
[ServiceFilter(typeof(SetExecutorFilter))]
public abstract class BaseController : ControllerBase
{
    protected readonly IExecutorService _executorService;

    public BaseController(IExecutorService executorService)
    {
        _executorService = executorService ?? throw new ArgumentNullException(nameof(executorService));
    }

    protected async Task<BaseAuthenticatedRequest> CreateBaseAuthenticatedRequest()
    {
        var executor = await _executorService.TryGet(User);

        if (executor is null)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        return new BaseAuthenticatedRequest { Executor = executor };
    }
}
