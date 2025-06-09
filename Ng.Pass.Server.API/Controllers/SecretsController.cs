using System;
using Microsoft.AspNetCore.Mvc;
using Ng.Pass.Server.Core.Models;
using Ng.Pass.Server.Services.Secrets.Models;
using Ng.Pass.Server.Services.Secrets.Services;
using Ng.Pass.Server.Services.Shared.Services;

namespace Ng.Pass.Server.API.Controllers;

[Route("api/[controller]")]
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
