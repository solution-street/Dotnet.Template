using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Ng.Pass.Server.Services.Secrets.Hubs;
using Ng.Pass.Server.Services.Shared.Extensions;

namespace Ng.Pass.Server.Services.Configurations;

public static class EndpointRouteBuilderExtensions
{
    public static void MapHubs(this IEndpointRouteBuilder endpoints)
    {
        // Map the SignalR hubs
        endpoints.MapHub<SecretsHub>(HubRouteExtensions.GetHubRoute<SecretsHub>());
    }
}
