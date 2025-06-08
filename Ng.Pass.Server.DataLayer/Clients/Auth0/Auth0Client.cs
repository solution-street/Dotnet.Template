using EWA.Coordination.Common.Logic.Extensions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Ng.Pass.Server.Core.Configuration;
using Ng.Pass.Server.Core.Exceptions;
using Ng.Pass.Server.DataLayer.Clients.Auth0.Models;

namespace Ng.Pass.Server.DataLayer.Clients.Auth0;

public class Auth0Client : IAuth0Client
{
    private readonly string _clientId;
    private readonly string _apiUrl;
    private readonly IHttpClientFactory _clientFactory;

    public Auth0Client(IConfiguration configuration, IHttpClientFactory clientFactory)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        ArgumentNullException.ThrowIfNull(clientFactory, nameof(clientFactory));

        _clientId = configuration.GetRequiredValue(AppConstants.EnvironmentVariables.AUTH0_CLIENT_ID);
        _apiUrl = configuration.GetRequiredValue(AppConstants.EnvironmentVariables.AUTH0_API_URL);
        _clientFactory = clientFactory;
    }

    public async Task TriggerPasswordResetEmail(string userEmail)
    {
        using var client = _clientFactory.CreateClient();

        PasswordResetRequest request = new PasswordResetRequest()
        {
            ClientId = _clientId,
            Email = userEmail,
            Connection = AppConstants.Auth0.PasswordResetConnection,
        };

        var apiEndpoint = $"{_apiUrl}/dbconnections/change_password";
        var apiRequest = new HttpRequestMessage(HttpMethod.Post, apiEndpoint);

        var content = new StringContent(JsonConvert.SerializeObject(request), null, "application/json");
        apiRequest.Content = content;

        var response = await client.SendAsync(apiRequest);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationLogicException(
                $"There was an error calling the Auth0 API for a password reset request for the email: {userEmail} \r\n HTTP Status Code: {response.StatusCode} \r\n Response Content: {response.Content}"
            );
        }
    }
}
