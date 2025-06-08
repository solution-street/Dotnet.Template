using Microsoft.AspNetCore.Mvc;
using Ng.Pass.Server.Services.Passwords.Models;
using Ng.Pass.Server.Services.Passwords.Services;

namespace Ng.Pass.Server.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PasswordsController : BaseController
{
    private readonly IPasswordService _passwordService;

    public PasswordsController(IPasswordService passwordService)
    {
        _passwordService = passwordService ?? throw new ArgumentNullException(nameof(passwordService));
    }

    [HttpPost]
    public async Task<CreatePasswordResponse> CreatePassword([FromBody] CreatePasswordRequest request)
    {
        return await _passwordService.CreatePassword(request);
    }

    [HttpPost("reveal")]
    public async Task<RevealPasswordResponse> RevealPassword([FromBody] RevealPasswordRequest request)
    {
        return await _passwordService.RevealAndDestroyPassword(request);
    }
}
