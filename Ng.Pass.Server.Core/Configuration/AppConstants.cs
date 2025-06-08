namespace Ng.Pass.Server.Core.Configuration;

public static class AppConstants
{
    /// <summary>
    /// Constants representing environment names used to determine application behavior.
    /// </summary>
    public static class Environment
    {
        /// <summary>
        /// Represents the local development environment.
        /// Used when running the application on a developer's machine.
        /// </summary>
        public const string Development = "Development";
    }

    /// <summary>
    /// Location to store application-specific environment variable constants.
    /// </summary>
    public static class EnvironmentVariables
    {
        /// <summary>
        /// Environment variable key that is holding the client's origin URL. Used for CORS policy construction.
        /// </summary>
        public const string ClientOriginUrlKey = "CLIENT_ORIGIN_URL";

        /// <summary>
        /// Environment variable key that is holding the key vault URI.
        /// </summary>
        public const string KeyVaultUri = "KeyVaultUri";

        /// <summary>
        /// Environment variable key that is holding the audience for the bearer token. In Auth0 this is the Client ID.
        /// </summary>
        public const string AuthCodeBearerTokenAudienceKey = "BEARER_TOKEN_AUDIENCE";

        /// <summary>
        /// Environment variable key that is holding the authority for the bearer token. In Auth0 this is the domain.
        /// </summary>
        public const string AuthCodeBearerTokenAuthorityKey = "BEARER_TOKEN_AUTHORITY";

        /// <summary>
        /// Environment variable key that is holding the base URL for the Auth0 API.
        /// </summary>
        public const string AUTH0_API_URL = "AUTH0_API_URL";

        /// <summary>
        /// Environment variable key that is holding the client ID for the Auth0 API.
        /// </summary>
        public const string AUTH0_CLIENT_ID = "AUTH0_CLIENT_ID";
    }

    /// <summary>
    /// Location to store application-specific secret constants.
    /// </summary>
    public static class Secrets
    {
        /// <summary>
        /// Secret key that is holding the connection string to the coordination database.
        /// </summary>
        public const string ConnectionStringKey = "NgPass";
    }

    public static class Authorization
    {
        /// <summary>
        /// The Session ID (sid) claim name used for Auth0.
        /// </summary>
        public const string SidClaim = "sid";
    }

    public static class Auth0
    {
        public const string PasswordResetConnection = "Username-Password-Authentication";
    }
}
