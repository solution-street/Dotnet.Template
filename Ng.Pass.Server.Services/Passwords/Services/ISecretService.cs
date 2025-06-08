using Ng.Pass.Server.Services.Secrets.Models;

namespace Ng.Pass.Server.Services.Secrets.Services;

public interface ISecretService
{
    Task<CreateSecretResponse> CreateSecret(CreateSecretRequest request);

    Task<RevealSecretResponse> RevealAndDisposeSecret(RevealSecretRequest request);
}
