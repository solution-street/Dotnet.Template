using Moq;
using Ng.Pass.Server.DataLayer.Clients.Auth0;

namespace Ng.Pass.Server.API.Tests.Helpers;

public class MockHelper
{
    public static Mock<IAuth0Client> GetAuth0ClientMock()
    {
        var mockAuth0Client = new Mock<IAuth0Client>();

        mockAuth0Client.Setup(c => c.TriggerPasswordResetEmail(It.IsAny<string>())).Returns(Task.CompletedTask);

        return mockAuth0Client;
    }
}
