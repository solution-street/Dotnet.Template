using Microsoft.Extensions.Configuration;

namespace Ng.Pass.Server.Core.Extensions;

public static class ConfigurationExtensions
{
    public static string GetRequiredValue(this IConfiguration configuration, string key)
    {
        return configuration[key] ?? throw new InvalidOperationException($"Configuration Key: '{key}' is required but not found.");
    }
}
