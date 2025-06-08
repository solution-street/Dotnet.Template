namespace Ng.Pass.Server.Services.Passwords.Models;

public class RevealPasswordRequest
{
    public Guid Guid { get; set; }
    public string Passphrase { get; set; } = null!;
}
