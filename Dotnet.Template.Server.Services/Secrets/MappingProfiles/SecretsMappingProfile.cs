using Dotnet.Template.Server.Database.Entities;
using Dotnet.Template.Server.Services.Secrets.Models;

namespace Dotnet.Template.Server.Services.Secrets.MappingProfiles;

public static class SecretsMappingProfile
{
    public static Secret ToEntity(this CreateSecretRequest request, string encryptedValue)
    {
        return new Secret
        {
            Id = Guid.NewGuid(),
            Ttl = request.Ttl,
            Value = encryptedValue,
            UserId = request.Executor?.UserId ?? null,
        };
    }

    public static CreateSecretResponse ToCreateResponse(this Secret secret)
    {
        return new CreateSecretResponse { Guid = secret.Id, CreatedAt = secret.CreatedAt };
    }

    public static RevealSecretResponse ToRevealResponse(this Secret secret, string decryptedValue)
    {
        return new RevealSecretResponse { Password = decryptedValue };
    }

    public static SecretGridResponse ToGridResponse(this Secret secret)
    {
        return new SecretGridResponse
        {
            Guid = secret.Id,
            Ttl = secret.Ttl,
            CreatedAt = secret.CreatedAt,
        };
    }
}
