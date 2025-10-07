using System.Security.Claims;
using Dotnet.Template.Server.Core.Models;

namespace Dotnet.Template.Server.Services.Shared.Services;

public interface IExecutorService
{
    /// <summary>
    /// Gets the current executor.
    /// </summary>
    /// <param name="principal"></param>
    /// <returns>The current executor.</returns>
    Task<Executor?> TryGet(ClaimsPrincipal principal);
}
