using Dotnet.Template.Server.Core.Models;

namespace Dotnet.Template.Server.Services.Secrets.Models;

public class CreateSecretRequest : BaseRequest
{
    public string Secret { get; set; } = null!;
    public string Ttl { get; set; } = null!;
    public string Passphrase { get; set; } = null!;
}
