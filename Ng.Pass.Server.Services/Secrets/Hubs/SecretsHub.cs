using Microsoft.AspNetCore.SignalR;
using Ng.Pass.Server.Core.Models;
using Ng.Pass.Server.Services.Secrets.Constants;
using Ng.Pass.Server.Services.Secrets.Services;
using Ng.Pass.Server.Services.Shared.Attributes;
using Ng.Pass.Server.Services.Shared.Services;

namespace Ng.Pass.Server.Services.Secrets.Hubs;

[HubRoute("/hubs/secrets")]
public class SecretsHub : Hub
{
    private readonly ISecretService _secretService;
    private readonly IExecutorService _executorService;

    public SecretsHub(ISecretService secretService, IExecutorService executorService)
    {
        _secretService = secretService ?? throw new ArgumentNullException(nameof(secretService));
        _executorService = executorService ?? throw new ArgumentNullException(nameof(executorService));
    }

    public async Task RequestSecretsList()
    {
        try
        {
            var userContext = Context.User;

            if (userContext is null)
            {
                throw new InvalidOperationException("User is not authenticated.");
            }

            var executor = await _executorService.TryGet(userContext);

            if (executor is null)
            {
                throw new InvalidOperationException("Executor not found for the authenticated user.");
            }

            BaseAuthenticatedRequest request = new BaseAuthenticatedRequest { Executor = executor };

            var secrets = await _secretService.GetSecretsCreatedByUser(request);

            await Clients.Caller.SendAsync(SecretHubConstants.Events.ListRecieved, secrets);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", $"Failed to fetch secrets: {ex.Message}");
        }
    }

    public async Task JoinSecretsGroup()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, SecretHubConstants.GroupName);
    }

    public async Task LeaveSecretsGroup()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, SecretHubConstants.GroupName);
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        await JoinSecretsGroup();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await LeaveSecretsGroup();
        await base.OnDisconnectedAsync(exception);
    }

    public async Task NotifySecretCreated(object secretData)
    {
        await Clients.Group(SecretHubConstants.GroupName).SendAsync(SecretHubConstants.Events.SecretCreated, secretData);
    }

    public async Task NotifySecretDeleted(int secretId)
    {
        await Clients.Group(SecretHubConstants.GroupName).SendAsync(SecretHubConstants.Events.SecretDeleted, secretId);
    }
}
