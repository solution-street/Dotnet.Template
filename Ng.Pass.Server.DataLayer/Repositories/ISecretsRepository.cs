using Ng.Pass.Server.Database.Contexts;
using Ng.Pass.Server.Database.Entities;

namespace Ng.Pass.Server.DataLayer.Repositories;

public interface ISecretsRepository
{
    Task<Secret?> TryGetByGuidAsync(Guid uid);

    Task<Secret> CreateAsync(Secret secret);

    Task<Secret> DeleteAsync(Secret secret);
}
