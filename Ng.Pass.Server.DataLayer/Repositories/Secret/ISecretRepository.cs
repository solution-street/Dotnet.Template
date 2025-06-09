namespace Ng.Pass.Server.DataLayer.Repositories.Secret;

public interface ISecretRepository
{
    Task<Database.Entities.Secret?> TryGetByGuidAsync(Guid uid);

    Task<IEnumerable<Database.Entities.Secret>> GetAllCreatedByUserAsync(Guid userId);

    Task<Database.Entities.Secret> CreateAsync(Database.Entities.Secret secret);

    Task<Database.Entities.Secret> DeleteAsync(Database.Entities.Secret secret);
}
