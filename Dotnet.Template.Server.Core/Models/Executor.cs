namespace Dotnet.Template.Server.Core.Models;

public class Executor
{
    public Guid UserId { get; }

    public Executor(Guid userId)
    {
        UserId = userId;
    }
}
