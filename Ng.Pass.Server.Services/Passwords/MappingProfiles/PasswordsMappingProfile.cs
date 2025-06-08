using Ng.Pass.Server.Database.Entities;
using Ng.Pass.Server.Services.Passwords.Models;

namespace Ng.Pass.Server.Services.Passwords.MappingProfiles;

public static class PasswordsMappingProfile
{
    public static Password ToEntity(this CreatePasswordRequest request, string encryptedValue)
    {
        return new Password
        {
            Id = Guid.NewGuid(),
            Ttl = request.Ttl,
            Value = encryptedValue,
        };
    }

    public static CreatePasswordResponse ToCreateResponse(this Password password)
    {
        return new CreatePasswordResponse { Guid = password.Id, CreatedAt = password.CreatedAt };
    }

    public static RevealPasswordResponse ToRevealResponse(this Password password, string decryptedValue)
    {
        return new RevealPasswordResponse { Password = decryptedValue, };
    }
}
