using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ng.Pass.Server.Core.Configuration;
using Ng.Pass.Server.Database.Contexts;

namespace Ng.Pass.Server.DataLayer.Configurations;

public static class ServiceCollectionExtensions
{
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<NgPassContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString(AppConstants.Secrets.ConnectionStringKey))
        );
    }

    /// <summary>
    /// Registers named HTTP clients with the dependency injection container to support specific API configurations.
    /// </summary>
    /// <remarks>
    /// This method sets up named clients using <see cref="IHttpClientFactory"/> to allow for customized HTTP handling per API.
    /// In addition, message handlers are added that force all HTTP requests to automatically go through custom logic to append
    /// authentication headers. This keeps the 3rd party API client code clean and free of authentication details.
    /// </remarks>
    /// <param name="services">The service collection to add the HTTP clients to.</param>
    public static void AddApiHttpClients(this IServiceCollection services)
    {
        //services.AddHttpClient(UlsApiConstants.HTTP_CLIENT_NAME).AddHttpMessageHandler<UlsAuthenticationHandler>();
        //services.AddHttpClient(AutoCoordApiConstants.HTTP_CLIENT_NAME).AddHttpMessageHandler<AutoCoordAuthenticationHandler>();
    }

    /// <summary>
    /// Registers authentication handler(s) with dependency injection for 3rd party API calls.
    /// </summary>
    /// <remarks>
    /// See: <see cref="AddApiHttpClients"/> for details on how these handlers are used.
    /// </remarks>
    /// <param name="services">The service collection to add the authentication handler(s) to.</param>
    public static void AddApiAuthenticationHandlers(this IServiceCollection services)
    {
        //services.AddScoped<UlsAuthenticationHandler>();
        //services.AddScoped<AutoCoordAuthenticationHandler>();
    }
}
