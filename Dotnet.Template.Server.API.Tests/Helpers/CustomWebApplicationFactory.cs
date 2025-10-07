using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Dotnet.Template.Server.API.Tests;
using Dotnet.Template.Server.Common.Tests;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
    where TStartup : class
{
    public Dependencies Dependencies { get; set; } = new Dependencies();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // This is overwriting the default Auth0 scheme and replacing it with a mocked version. If we have more schemes in the future
            // they may need to be added here.
            services
                .AddAuthentication(TestConstants.Auth.TestScheme)
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestConstants.Auth.TestScheme, options => { });

            ReplaceDbContext(services);

            //services.AddScoped(_ => Dependencies.Auth0Client.Object);

            var sp = services.BuildServiceProvider();
            //using (var scope = sp.CreateScope())
            //{
            //    var scopedServices = scope.ServiceProvider;
            //    var db = scopedServices.GetRequiredService<CoordinationContext>();
            //    db.Database.EnsureCreated();
            //}
        });
    }

    /// <summary>
    /// Removes the existing (real) CoordinationContext registration (if it exists) and swaps for an in-memory database.
    /// </summary>
    /// <param name="services"></param>
    private static void ReplaceDbContext(IServiceCollection services)
    {
        //var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<CoordinationContext>));
        //if (descriptor != null)
        //{
        //    services.Remove(descriptor);
        //}

        // Using a unique DB name to ensure that each test has its own database when tests are running in parallel.
        // Each test will clean up its own DB instance after test completion.
        var uniqueDbName = Guid.NewGuid().ToString();
        //services.AddDbContext<CoordinationContext>(options =>
        //{
        //    options.UseInMemoryDatabase(uniqueDbName);
        //});
    }

    /// <summary>
    /// Overrides an existing authentication scheme with a custom test handler for testing purposes.
    /// This ensures that when a specific scheme is requested during authentication, it uses the mocked handler.
    /// </summary>
    /// <param name="services">The service collection to apply the override to.</param>
    /// <param name="schemeName">The name of the authentication scheme to override.</param>
    private static void OverrideAuthenticationSchemeWithTestHandler(IServiceCollection services, string schemeName)
    {
        services.PostConfigure<AuthenticationOptions>(authenticationOptions =>
        {
            var existingScheme = authenticationOptions.Schemes.FirstOrDefault(scheme => scheme.Name == schemeName);
            if (existingScheme != null)
            {
                existingScheme.HandlerType = typeof(TestAuthHandler);

                var overriddenSchemeBuilder = new AuthenticationSchemeBuilder(schemeName) { HandlerType = typeof(TestAuthHandler) };

                authenticationOptions.SchemeMap[schemeName] = overriddenSchemeBuilder;
            }
        });
    }
}
