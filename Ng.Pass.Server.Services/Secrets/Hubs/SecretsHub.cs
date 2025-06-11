using Microsoft.AspNetCore.SignalR;
using Ng.Pass.Server.Core.Models;
using Ng.Pass.Server.Services.Secrets.Services;

namespace Ng.Pass.Server.Services.Secrets.Hubs;

public class SecretsHub : Hub
{
    public readonly string HubName = "SecretsHub";

    private readonly ISecretService _secretService;

    public SecretsHub(ISecretService secretService)
    {
        _secretService = secretService;
    }

    public async Task RequestSecretsList()
    {
        try
        {
            BaseAuthenticatedRequest request = new BaseAuthenticatedRequest
            {
                Executor = new Executor(new Guid("14c6de87-6202-426f-8c28-ba013d61c9ad"))
            };

            var secrets = await _secretService.GetSecretsCreatedByUser(request);

            await Clients.Caller.SendAsync("SecretsListReceived", secrets);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", $"Failed to fetch secrets: {ex.Message}");
        }
    }

    public async Task JoinSecretsGroup()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "SecretsGroup");
    }

    public async Task LeaveSecretsGroup()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SecretsGroup");
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
        await Clients.Group("SecretsGroup").SendAsync("SecretCreated", secretData);
    }

    public async Task NotifySecretDeleted(int secretId)
    {
        await Clients.Group("SecretsGroup").SendAsync("SecretDeleted", secretId);
    }
}
