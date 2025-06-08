using Newtonsoft.Json;

namespace Ng.Pass.Server.DataLayer.Clients.Auth0.Models;

public class PasswordResetRequest
{
    [JsonProperty("client_id")]
    public required string ClientId { get; set; }

    [JsonProperty("email")]
    public required string Email { get; set; }

    [JsonProperty("connection")]
    public required string Connection { get; set; }
}
