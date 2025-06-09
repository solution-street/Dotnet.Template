using Ng.Pass.Server.Core.Models;

namespace Ng.Pass.Server.Services.Shared.MappingProfiles;

public static class ExecutorMappingProfile
{
    public static Executor ToExecutor(this Database.Entities.User user)
    {
        return new Executor(user.Id);
    }
}
