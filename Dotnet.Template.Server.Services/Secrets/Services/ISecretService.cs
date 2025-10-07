using Dotnet.Template.Server.Core.Models;
using Dotnet.Template.Server.Services.Secrets.Models;

namespace Dotnet.Template.Server.Services.Secrets.Services;

public interface ISecretService
{
    Task<CreateSecretResponse> CreateSecret(CreateSecretRequest request);

    Task<RevealSecretResponse> RevealAndDisposeSecret(RevealSecretRequest request);

    Task<IEnumerable<SecretGridResponse>> GetSecretsCreatedByUser(BaseAuthenticatedRequest request);
}
