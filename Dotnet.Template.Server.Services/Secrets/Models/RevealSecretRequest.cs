namespace Dotnet.Template.Server.Services.Secrets.Models;

public class RevealSecretRequest
{
    public Guid Guid { get; set; }
    public string Passphrase { get; set; } = null!;
}
