using System.Reflection;
using Microsoft.AspNetCore.SignalR;
using Ng.Pass.Server.Services.Shared.Attributes;

namespace Ng.Pass.Server.Services.Shared.Extensions;

public class HubRouteExtensions
{
    /// <summary>
    /// Gets the route from HubRouteAttribute
    /// </summary>
    public static string GetHubRoute<THub>()
        where THub : Hub
    {
        return GetHubRoute(typeof(THub));
    }

    /// <summary>
    /// Gets the route from HubRouteAttribute
    /// </summary>
    public static string GetHubRoute(Type hubType)
    {
        var attribute = hubType.GetCustomAttribute<HubRouteAttribute>();

        if (attribute == null)
        {
            throw new InvalidOperationException($"Hub {hubType.Name} does not have HubRouteAttribute");
        }

        return attribute.RouteName;
    }
}
