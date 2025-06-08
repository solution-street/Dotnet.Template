using Microsoft.AspNetCore.Mvc;
using Ng.Pass.Server.Services.Secrets.Models;
using Ng.Pass.Server.Services.Secrets.Services;

namespace Ng.Pass.Server.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SecretsController : BaseController
{
    private readonly ISecretService _secretService;

    public SecretsController(ISecretService secretService)
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
}
