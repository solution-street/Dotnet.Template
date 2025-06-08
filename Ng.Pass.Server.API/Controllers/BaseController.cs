using Microsoft.AspNetCore.Mvc;

namespace Ng.Pass.Server.API.Controllers;

/// <summary>
/// Base controller that all application controllers inherit from. This ensures proper audit logging and access to common services.
/// </summary>
//[ServiceFilter(typeof(SetExecutorAuditActionFilter))]
public abstract class BaseController : ControllerBase
{
    //protected readonly IExecutorService _executorService;

    //public BaseController(IExecutorService executorService)
    //{
    //    _executorService = executorService ?? throw new ArgumentNullException(nameof(executorService));
    //}
}
