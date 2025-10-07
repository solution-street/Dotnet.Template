using System.Security.Claims;
using Dotnet.Template.Server.API.Authorization.Extensions;
using Dotnet.Template.Server.Core.Models;
using Dotnet.Template.Server.DataLayer.Repositories.User;
using Dotnet.Template.Server.Services.Shared.MappingProfiles;

namespace Dotnet.Template.Server.Services.Shared.Services;

public class ExecutorService : IExecutorService
{
    private readonly IUserRepository _userRepository;

    public ExecutorService(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<Executor?> TryGet(ClaimsPrincipal principal)
    {
        string? authProviderId = principal.TryGetName();
        if (string.IsNullOrWhiteSpace(authProviderId))
        {
            return null; // No authenticated user
        }

        Database.Entities.User? userEntity = await _userRepository.TryFindByAuthProviderIdAsync(authProviderId);

        if (userEntity is null)
        {
            userEntity = new Database.Entities.User { Id = Guid.NewGuid(), AuthProviderId = authProviderId, };

            userEntity = await _userRepository.CreateAsync(userEntity);
        }

        return userEntity.ToExecutor();
    }
}
