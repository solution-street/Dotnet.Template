using Microsoft.EntityFrameworkCore;
using Ng.Pass.Server.Database.Contexts;

namespace Ng.Pass.Server.DataLayer.Repositories.User;

public class UserRepository : IUserRepository
{
    private readonly NgPassContext _context;

    public UserRepository(NgPassContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Database.Entities.User?> TryFindByAuthProviderIdAsync(string authProviderId)
    {
        if (string.IsNullOrWhiteSpace(authProviderId))
        {
            throw new ArgumentException("Auth provider ID cannot be null or empty.", nameof(authProviderId));
        }

        return await _context.Users.FirstOrDefaultAsync(u => u.AuthProviderId == authProviderId);
    }

    public async Task<Database.Entities.User> CreateAsync(Database.Entities.User user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return user;
    }
}
