using System.Security.Claims;
using Ng.Pass.Server.Core.Configuration;

namespace Ng.Pass.Server.API.Authorization.Extensions;

public static class AuthExtensions
{
    /// <summary>
    /// Returns the name identifier from the claims principal. This maps to the Auth0 id.
    /// </summary>
    /// <param name="principle"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static string GetName(this ClaimsPrincipal principle)
    {
        Claim nameClaim =
            principle.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException($"Auth provider claim ({nameof(ClaimTypes.NameIdentifier)}) is missing.");

        return nameClaim.Value;
    }

    /// <summary>
    /// Returns the session ID (sid) from the claims principal.
    /// </summary>
    /// <param name="principle"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static string GetSessionId(this ClaimsPrincipal principle)
    {
        Claim sidClaim =
            principle.Claims.FirstOrDefault(p => p.Type == AppConstants.Authorization.SidClaim)
            ?? throw new UnauthorizedAccessException($"Session ID (sid) claim {nameof(AppConstants.Authorization.SidClaim)} is missing.");

        return sidClaim.Value;
    }
}
