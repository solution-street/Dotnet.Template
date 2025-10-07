using Dotnet.Template.Server.API.Responses;
using Dotnet.Template.Server.Core.Enums;
using Dotnet.Template.Server.Core.Extensions;
using Dotnet.Template.Server.Services.Secrets.Models;

namespace Dotnet.Template.Server.API.Tests.Controllers.Secrets;

public class CreateSecretTests : BaseTests
{
    protected override string _controllerRoute => "/api/secrets";

    [Fact]
    public async Task CreateSecret_ValidRequest_ShouldSucceed()
    {
        // Arrange
        var request = new CreateSecretRequest
        {
            Secret = "Secret value",
            Ttl = TimeToLive.Day.GetStringValue(),
            Passphrase = "StrongPassphrase123!"
        };

        // Act
        var response = await SendPostRequest<CreateSecretRequest, CreateSecretResponse>(string.Empty, request);

        // Assert
        Assert.NotNull(response);
    }

    [Fact]
    public async Task CreateSecret_InvalidRequest_ShouldReturnException()
    {
        // Arrange
        var request = new CreateSecretRequest
        {
            Secret = "", // Invalid: empty secret
            Ttl = "1h",
            Passphrase = "StrongPassphrase123!"
        };

        // Act
        var response = await SendPostRequest<CreateSecretRequest, ApiError>(string.Empty, request);

        // Assert
        response.Should().NotBeNull();
        response.Code.Should().Be(ErrorCode.ValidationFailure.ToString());
    }
}
