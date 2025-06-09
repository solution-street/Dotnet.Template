using System.Security.Claims;
using Ng.Pass.Server.API.Authorization.Extensions;
using Ng.Pass.Server.Core.Models;
using Ng.Pass.Server.DataLayer.Repositories.User;
using Ng.Pass.Server.Services.Shared.MappingProfiles;

namespace Ng.Pass.Server.Services.Shared.Services;

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
