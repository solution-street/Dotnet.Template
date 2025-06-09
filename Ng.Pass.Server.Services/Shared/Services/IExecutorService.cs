using System.Security.Claims;
using Ng.Pass.Server.Core.Models;

namespace Ng.Pass.Server.Services.Shared.Services;

public interface IExecutorService
{
    /// <summary>
    /// Gets the current executor.
    /// </summary>
    /// <param name="principal"></param>
    /// <returns>The current executor.</returns>
    Task<Executor?> TryGet(ClaimsPrincipal principal);
}
