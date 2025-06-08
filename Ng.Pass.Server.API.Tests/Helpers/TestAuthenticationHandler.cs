using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private static ClaimsPrincipal User = new();

    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
        : base(options, logger, encoder) { }

    public static void AddUser(string authId)
    {
        //var principal = ExecutorHelper.CreateCustomerIdentity(authId);
        //User = principal;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        // If there is any bearer token, regardless of value, consider that a success. These tests are meant to
        // surpass the Auth0 bearer token verification logic.
        if (authHeader.ToString().StartsWith("Bearer "))
        {
            var ticket = new AuthenticationTicket(User, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        return Task.FromResult(AuthenticateResult.Fail("Invalid token. Must be formatted like: 'Bearer {token}.'"));
    }
}
