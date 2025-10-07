using System.Security.Claims;

namespace Dotnet.Template.Server.API.Authorization.Extensions;

public static class AuthExtensions
{
    /// <summary>
    /// Returns the name identifier from the claims principal. This maps to the Auth0 id.
    /// </summary>
    /// <param name="principle"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static string? TryGetName(this ClaimsPrincipal principle)
    {
        return principle.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}
