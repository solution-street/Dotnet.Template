using Ng.Pass.Server.Core.Enums;

namespace Ng.Pass.Server.Core.Models;

public class Executor
{
    public int UserId { get; }
    public string UserEmail { get; } = null!;
    public string Username { get; } = null!;

    public Executor(int userId, string userEmail, string username)
    {
        UserId = userId;
        UserEmail = userEmail;
        Username = username;
    }
}
