using System.Security.Claims;
using Ng.Pass.Server.Core.Configuration;
using Ng.Pass.Server.Core.Enums;

namespace Ng.Pass.Server.Common.Tests.Helpers;

public static class ExecutorHelper
{
    //public static Executor GetTestExecutor(
    //    UserRole userRole,
    //    int userId = 1,
    //    string userEmail = "user@enterprisewireless.org",
    //    string username = "mytestuser"
    //)
    //{
    //    return new Executor(userId: userId, userEmail: userEmail, userRole: userRole, username: username);
    //}

    //public static ClaimsPrincipal CreateCustomerIdentity(string authProviderId)
    //{
    //    var nameIdentifier = new Claim(ClaimTypes.NameIdentifier, authProviderId);
    //    var sessionId = new Claim(AppConstants.Authorization.SidClaim, TestConstants.Auth.Claims.SidClaim);
    //    var identity = new ClaimsIdentity(new[] { nameIdentifier, sessionId }, TestConstants.Auth.TestScheme);

    //    return new ClaimsPrincipal(identity);
    //}
}
