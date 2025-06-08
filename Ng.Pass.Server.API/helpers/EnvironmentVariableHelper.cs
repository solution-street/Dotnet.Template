using EWA.Coordination.Common.Logic.Extensions;
using Ng.Pass.Server.Core.Configuration;

namespace Ng.Pass.Server.API.Helpers;

public static class EnvironmentVariableHelper
{
    /// <summary>
    /// Retrieves <see cref="ClientOriginUrlKey"/> environment variable value.
    /// </summary>
    /// <returns>Comma-separated list of acceptable client URLs.</returns>
    public static string[] GetClientOriginUrls()
    {
        return GetEnvironmentVariableValue(AppConstants.EnvironmentVariables.ClientOriginUrlKey).Split(',');
    }

    public static string GetKeyVaultUri()
    {
        return GetEnvironmentVariableValue(AppConstants.EnvironmentVariables.KeyVaultUri);
    }

    public static string GetEnvironmentName()
    {
        return GetEnvironmentVariableValue("ASPNETCORE_ENVIRONMENT");
    }

    public static bool IsDevelopment()
    {
        return GetEnvironmentName().EqualsIgnoreCasing(AppConstants.Environment.Development);
    }

    private static string GetEnvironmentVariableValue(string key)
    {
        return Environment.GetEnvironmentVariable(key)
            ?? throw new InvalidOperationException($"Environment Key: {key} does not exist in environment variables.");
    }

    /// <summary>
    /// Gets the authorization code flow authority for the bearer token.
    /// </summary>
    /// <returns></returns>
    public static string GetAuthCodeBearerTokenAudience()
    {
        return GetEnvironmentVariableValue(AppConstants.EnvironmentVariables.AuthCodeBearerTokenAudienceKey);
    }

    /// <summary>
    /// Gets the authorization code flow audience for the bearer token.
    /// </summary>
    /// <returns></returns>
    public static string GetAuthCodeBearerTokenAuthority()
    {
        return GetEnvironmentVariableValue(AppConstants.EnvironmentVariables.AuthCodeBearerTokenAuthorityKey);
    }
}
