namespace Ng.Pass.Server.DataLayer.Repositories.User;

public interface IUserRepository
{
    Task<Database.Entities.User?> TryFindByAuthProviderIdAsync(string authProviderId);

    Task<Database.Entities.User> CreateAsync(Database.Entities.User user);
}
