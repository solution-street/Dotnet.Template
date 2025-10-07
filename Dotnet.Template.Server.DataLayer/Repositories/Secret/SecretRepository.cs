using Dotnet.Template.Server.Database.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Template.Server.DataLayer.Repositories.Secret;

public class SecretRepository : ISecretRepository
{
    private readonly AppDbContext _context;

    public SecretRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Database.Entities.Secret?> TryGetByGuidAsync(Guid uid)
    {
        if (uid == Guid.Empty)
        {
            throw new ArgumentException("The provided GUID is empty.", nameof(uid));
        }

        return await _context.Secrets.FindAsync(uid);
    }

    public async Task<IEnumerable<Database.Entities.Secret>> GetAllCreatedByUserAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("The provided user ID is empty.", nameof(userId));
        }

        return await _context.Secrets.Where(secret => secret.UserId == userId).ToListAsync();
    }

    public async Task<Database.Entities.Secret> CreateAsync(Database.Entities.Secret secret)
    {
        ArgumentNullException.ThrowIfNull(secret, nameof(secret));

        await _context.Secrets.AddAsync(secret);

        await _context.SaveChangesAsync();

        return secret;
    }

    public async Task<Database.Entities.Secret> DeleteAsync(Database.Entities.Secret secret)
    {
        ArgumentNullException.ThrowIfNull(secret, nameof(secret));

        _context.Secrets.Remove(secret);

        await _context.SaveChangesAsync();

        return secret;
    }
}
