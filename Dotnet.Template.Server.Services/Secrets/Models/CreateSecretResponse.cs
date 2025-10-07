namespace Dotnet.Template.Server.Services.Secrets.Models;

public class CreateSecretResponse
{
    public Guid Guid { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
