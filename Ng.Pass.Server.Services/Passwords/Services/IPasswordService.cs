using Ng.Pass.Server.Services.Passwords.Models;

namespace Ng.Pass.Server.Services.Passwords.Services;

public interface IPasswordService
{
    Task<CreatePasswordResponse> CreatePassword(CreatePasswordRequest request);

    Task<RevealPasswordResponse> RevealAndDestroyPassword(RevealPasswordRequest request);
}
