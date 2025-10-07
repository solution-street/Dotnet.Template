using Dotnet.Template.Server.API.Responses;
using Dotnet.Template.Server.Core.Enums;
using Dotnet.Template.Server.Core.Extensions;
using Dotnet.Template.Server.Services.Encryption.Services;
using Dotnet.Template.Server.Services.Secrets.Models;

namespace Dotnet.Template.Server.API.Tests.Controllers.Secrets;

public class RevealSecretTests : BaseTests
{
    protected override string _controllerRoute => "/api/secrets/reveal";

    private readonly EncryptionService _encryptionService;

    public RevealSecretTests()
    {
        _encryptionService = new EncryptionService();
    }

    [Fact]
    public async Task RevealSecret_SecretIsInDatabaseAndValidRequest_ResponseReturnsSecret()
    {
        // Arrange
        var guid = Guid.NewGuid();

        var secret = "Secret value";
        var passphrase = "StrongPassphrase123!";

        Context.Secrets.Add(
            new Database.Entities.Secret
            {
                Id = guid,
                CreatedAt = DateTime.UtcNow,
                Ttl = TimeToLive.Day.GetStringValue(),
                Value = _encryptionService.Encrypt(secret, passphrase),
            }
        );

        await Context.SaveChangesAsync();

        var request = new RevealSecretRequest { Guid = guid, Passphrase = passphrase };

        // Act
        var response = await SendPostRequest<RevealSecretRequest, RevealSecretResponse>(string.Empty, request);

        // Assert
        response.Password.Should().Be(secret);

        var secretInDatabase = Context.Secrets.Where(x => x.Id == guid).FirstOrDefault();

        secretInDatabase.Should().BeNull();
    }

    [Fact]
    public async Task RevealSecret_SecretIsExpired_ReturnsException()
    {
        // Arrange
        var guid = Guid.NewGuid();

        var secret = "Secret value";
        var passphrase = "StrongPassphrase123!";

        Context.Secrets.Add(
            new Database.Entities.Secret
            {
                Id = guid,
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                Ttl = TimeToLive.Day.GetStringValue(),
                Value = _encryptionService.Encrypt(secret, passphrase),
            }
        );

        await Context.SaveChangesAsync();

        var request = new RevealSecretRequest { Guid = guid, Passphrase = passphrase };

        // Act
        var response = await SendPostRequest<RevealSecretRequest, ApiError>(string.Empty, request);

        // Assert
        response.Should().NotBeNull();
        response.Code.Should().Be(ErrorCode.InternalServerError.ToString());
    }
}
