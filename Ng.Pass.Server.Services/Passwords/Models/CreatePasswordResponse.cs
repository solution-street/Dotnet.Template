namespace Ng.Pass.Server.Services.Passwords.Models;

public class CreatePasswordResponse
{
    public Guid Guid { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
