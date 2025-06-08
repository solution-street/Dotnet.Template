using Microsoft.AspNetCore.Mvc.Testing;
using Ng.Pass.Server.Common.Tests;
using Ng.Pass.Server.Core.Configuration;

namespace Ng.Pass.Server.API.Tests.Helpers;

public static class TestSetupHelpers
{
    public static WebApplicationFactory<Startup> CreateWebApplicationFactory()
    {
        return new WebApplicationFactory<Startup>();
    }

    public static HttpClient CreateTestServerClient(WebApplicationFactory<Startup> webFactory)
    {
        return webFactory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    }

    public static void SetEnvironmentVariables()
    {
        Environment.SetEnvironmentVariable(AppConstants.EnvironmentVariables.ClientOriginUrlKey, "https://localhost:7097");
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", AppConstants.Environment.Development);

        // These values won't actually be used, but MUST be non-null in order for the environment variable retrieval code to
        // not throw an error.
        Environment.SetEnvironmentVariable(AppConstants.EnvironmentVariables.AuthCodeBearerTokenAudienceKey, "fakeValue");
        Environment.SetEnvironmentVariable(AppConstants.EnvironmentVariables.AuthCodeBearerTokenAuthorityKey, "fakeValue");
        Environment.SetEnvironmentVariable(AppConstants.EnvironmentVariables.AUTH0_CLIENT_ID, "fakeValue");
        Environment.SetEnvironmentVariable(AppConstants.EnvironmentVariables.AUTH0_API_URL, "fakeValue");
    }
}
