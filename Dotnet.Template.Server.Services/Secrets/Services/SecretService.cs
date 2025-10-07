using FluentValidation;
using Microsoft.AspNetCore.SignalR;
using Dotnet.Template.Server.Core.Enums;
using Dotnet.Template.Server.Core.Exceptions;
using Dotnet.Template.Server.Core.Extensions;
using Dotnet.Template.Server.Core.Models;
using Dotnet.Template.Server.Database.Entities;
using Dotnet.Template.Server.DataLayer.Repositories.Secret;
using Dotnet.Template.Server.Services.Encryption.Services;
using Dotnet.Template.Server.Services.Secrets.Hubs;
using Dotnet.Template.Server.Services.Secrets.MappingProfiles;
using Dotnet.Template.Server.Services.Secrets.Models;

namespace Dotnet.Template.Server.Services.Secrets.Services;

public class SecretService : ISecretService
{
    private readonly IEncryptionService _encryptionService;

    private readonly ISecretRepository _secretsRepository;

    private readonly IValidator<CreateSecretRequest> _createSecretValidator;
    private readonly IValidator<RevealSecretRequest> _revealSecretValidator;

    private readonly IHubContext<SecretsHub> _hubContext;

    public SecretService(
        IEncryptionService encryptionService,
        ISecretRepository secretsRepository,
        IValidator<CreateSecretRequest> createSecretValidator,
        IValidator<RevealSecretRequest> revealSecretValidator,
        IHubContext<SecretsHub> hubContext
    )
    {
        _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
        _secretsRepository = secretsRepository ?? throw new ArgumentNullException(nameof(secretsRepository));
        _createSecretValidator = createSecretValidator ?? throw new ArgumentNullException(nameof(createSecretValidator));
        _revealSecretValidator = revealSecretValidator ?? throw new ArgumentNullException(nameof(revealSecretValidator));
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
    }

    public async Task<CreateSecretResponse> CreateSecret(CreateSecretRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        await _createSecretValidator.ValidateAndThrowAsync(request);

        string encryptedValue = _encryptionService.Encrypt(request.Secret, request.Passphrase);

        var secret = await _secretsRepository.CreateAsync(request.ToEntity(encryptedValue));

        await _hubContext.Clients.Group("SecretsGroup").SendAsync("SecretCreated", new { secret.Id });

        return secret.ToCreateResponse();
    }

    public async Task<RevealSecretResponse> RevealAndDisposeSecret(RevealSecretRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        await _revealSecretValidator.ValidateAndThrowAsync(request);

        var secret = await _secretsRepository.TryGetByGuidAsync(request.Guid);

        if (secret == null)
        {
            throw new KeyNotFoundException($"Secret with GUID {request.Guid} not found.");
        }

        var latestTime = GetAtTheLatest(secret);
        if (DateTime.UtcNow > latestTime)
        {
            throw new ApplicationLogicException(
                $"The secret with GUID {request.Guid} has expired. It was created at {secret.CreatedAt} and is valid until {latestTime}."
            );
        }

        var decryptedValue = _encryptionService.Decrypt(secret.Value, request.Passphrase);

        var response = secret.ToRevealResponse(decryptedValue);

        await _secretsRepository.DeleteAsync(secret);

        await _hubContext.Clients.Group("SecretsGroup").SendAsync("SecretDeleted", new { secret.Id });

        return response;
    }

    public async Task<IEnumerable<SecretGridResponse>> GetSecretsCreatedByUser(BaseAuthenticatedRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var secrets = await _secretsRepository.GetAllCreatedByUserAsync(request.Executor!.UserId);

        return secrets.Select(secrets => secrets.ToGridResponse()).ToList();
    }

    private DateTime GetAtTheLatest(Secret secret)
    {
        var timeToLive = secret.Ttl;

        if (string.Equals(timeToLive, TimeToLive.Hour.GetStringValue()))
        {
            return secret.CreatedAt.AddHours(1);
        }
        if (string.Equals(timeToLive, TimeToLive.Day.GetStringValue()))
        {
            return secret.CreatedAt.AddDays(1);
        }
        if (string.Equals(timeToLive, TimeToLive.Week.GetStringValue()))
        {
            return secret.CreatedAt.AddHours(1);
        }

        throw new ApplicationLogicException($"The 'Ttl' field was not recognized as a valid TimeToLive value: {timeToLive}.");
    }
}
