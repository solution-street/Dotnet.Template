using Microsoft.AspNetCore.Mvc;
using Dotnet.Template.Server.Core.Models;
using Dotnet.Template.Server.Services.Secrets.Models;
using Dotnet.Template.Server.Services.Secrets.Services;
using Dotnet.Template.Server.Services.Shared.Services;

namespace Dotnet.Template.Server.API.Controllers;

[Route("api/secrets")]
[ApiController]
public class SecretsController : BaseController
{
    private readonly ISecretService _secretService;

    public SecretsController(ISecretService secretService, IExecutorService executorService)
        : base(executorService)
    {
        _secretService = secretService ?? throw new ArgumentNullException(nameof(secretService));
    }

    [HttpPost]
    public async Task<CreateSecretResponse> CreateSecret([FromBody] CreateSecretRequest request)
    {
        return await _secretService.CreateSecret(request);
    }

    [HttpPost("reveal")]
    public async Task<RevealSecretResponse> RevealSecret([FromBody] RevealSecretRequest request)
    {
        return await _secretService.RevealAndDisposeSecret(request);
    }

    [HttpGet()]
    public async Task<IEnumerable<SecretGridResponse>> GetSecretsCreatedByUser()
    {
        BaseAuthenticatedRequest request = await CreateBaseAuthenticatedRequest();

        return await _secretService.GetSecretsCreatedByUser(request);
    }
}
