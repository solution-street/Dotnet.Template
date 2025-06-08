using Ng.Pass.Server.Database.Contexts;
using Ng.Pass.Server.Database.Entities;

namespace Ng.Pass.Server.DataLayer.Repositories;

public class PasswordsRepository : IPasswordsRepository
{
    private readonly NgPassContext _context;

    public PasswordsRepository(NgPassContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Password?> TryGetByGuidAsync(Guid uid)
    {
        if (uid == Guid.Empty)
        {
            throw new ArgumentException("The provided GUID is empty.", nameof(uid));
        }

        return await _context.Passwords.FindAsync(uid);
    }

    public async Task<Password> CreateAsync(Password password)
    {
        ArgumentNullException.ThrowIfNull(password, nameof(password));

        await _context.Passwords.AddAsync(password);

        await _context.SaveChangesAsync();

        return password;
    }

    public async Task<Password> DeleteAsync(Password password)
    {
        ArgumentNullException.ThrowIfNull(password, nameof(password));

        _context.Passwords.Remove(password);

        await _context.SaveChangesAsync();

        return password;
    }
}
