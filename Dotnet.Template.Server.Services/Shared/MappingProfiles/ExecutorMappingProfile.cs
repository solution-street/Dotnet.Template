using Dotnet.Template.Server.Core.Models;

namespace Dotnet.Template.Server.Services.Shared.MappingProfiles;

public static class ExecutorMappingProfile
{
    public static Executor ToExecutor(this Database.Entities.User user)
    {
        return new Executor(user.Id);
    }
}
