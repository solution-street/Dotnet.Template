namespace Ng.Pass.Server.DataLayer.Clients.Auth0;

public interface IAuth0Client
{
    /// <summary>
    /// API call to Auth0 to trigger a password reset for the user. User will receive an email with a link to reset their password
    /// directly from Auth0.
    /// </summary>
    /// <returns></returns>
    Task TriggerPasswordResetEmail(string userEmail);
}
