namespace Ng.Pass.Server.Services.Secrets.Models;

public class RevealSecretRequest
{
    public Guid Guid { get; set; }
    public string Passphrase { get; set; } = null!;
}
