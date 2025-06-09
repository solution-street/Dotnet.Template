namespace Ng.Pass.Server.Services.Secrets.Models;

public class SecretGridResponse
{
    public Guid Guid { get; set; }
    public string Ttl { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
