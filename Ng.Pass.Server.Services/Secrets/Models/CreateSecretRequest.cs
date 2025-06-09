using Ng.Pass.Server.Core.Models;

namespace Ng.Pass.Server.Services.Secrets.Models;

public class CreateSecretRequest : BaseRequest
{
    public string Secret { get; set; } = null!;
    public string Ttl { get; set; } = null!;
    public string Passphrase { get; set; } = null!;
}
