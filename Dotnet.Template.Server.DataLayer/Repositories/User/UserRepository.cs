using Dotnet.Template.Server.Database.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Template.Server.DataLayer.Repositories.User;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
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
