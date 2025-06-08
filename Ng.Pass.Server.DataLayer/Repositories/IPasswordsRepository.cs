using Ng.Pass.Server.Database.Contexts;
using Ng.Pass.Server.Database.Entities;

namespace Ng.Pass.Server.DataLayer.Repositories;

public interface IPasswordsRepository
{
    Task<Password?> TryGetByGuidAsync(Guid uid);

    Task<Password> CreateAsync(Password password);

    Task<Password> DeleteAsync(Password password);
}
