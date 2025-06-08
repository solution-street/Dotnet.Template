using Ng.Pass.Server.Database.Contexts;
using Ng.Pass.Server.Database.Entities;

namespace Ng.Pass.Server.DataLayer.Repositories;

public class SecretsRepository : ISecretsRepository
{
    private readonly NgPassContext _context;

    public SecretsRepository(NgPassContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Secret?> TryGetByGuidAsync(Guid uid)
    {
        if (uid == Guid.Empty)
        {
            throw new ArgumentException("The provided GUID is empty.", nameof(uid));
        }

        return await _context.Secrets.FindAsync(uid);
    }

    public async Task<Secret> CreateAsync(Secret secret)
    {
        ArgumentNullException.ThrowIfNull(secret, nameof(secret));

        await _context.Secrets.AddAsync(secret);

        await _context.SaveChangesAsync();

        return secret;
    }

    public async Task<Secret> DeleteAsync(Secret secret)
    {
        ArgumentNullException.ThrowIfNull(secret, nameof(secret));

        _context.Secrets.Remove(secret);

        await _context.SaveChangesAsync();

        return secret;
    }
}
