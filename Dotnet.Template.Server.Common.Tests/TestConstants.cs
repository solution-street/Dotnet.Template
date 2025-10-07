namespace Dotnet.Template.Server.Common.Tests;

public static class TestConstants
{
    public static class Auth
    {
        public static class AuthProviderId
        {
            public const string Customer = "auth0|customer";
            public const string Admin = "auth0|admin";
        }

        public static class Claims
        {
            public const string SidClaim = "test-session-id";
        }

        /// <summary>
        /// The test scheme used for integration tests and construction of claims.
        /// </summary>
        public const string TestScheme = "TestScheme";
    }
}
