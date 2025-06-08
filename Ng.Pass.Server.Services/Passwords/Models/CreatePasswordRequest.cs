namespace Ng.Pass.Server.Services.Passwords.Models;

public class CreatePasswordRequest
{
    public string Secret { get; set; } = null!;
    public string Ttl { get; set; } = null!;
    public string Passphrase { get; set; } = null!;
}
