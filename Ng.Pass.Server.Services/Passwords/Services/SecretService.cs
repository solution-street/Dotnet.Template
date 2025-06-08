using FluentValidation;
using Ng.Pass.Server.Core.Enums;
using Ng.Pass.Server.Core.Exceptions;
using Ng.Pass.Server.Core.Extensions;
using Ng.Pass.Server.Database.Entities;
using Ng.Pass.Server.DataLayer.Repositories;
using Ng.Pass.Server.Services.Encryption.Services;
using Ng.Pass.Server.Services.Secrets.MappingProfiles;
using Ng.Pass.Server.Services.Secrets.Models;

namespace Ng.Pass.Server.Services.Secrets.Services;

public class SecretService : ISecretService
{
    private readonly IEncryptionService _encryptionService;

    private readonly ISecretsRepository _secretsRepository;

    private readonly IValidator<CreateSecretRequest> _createSecretValidator;
    private readonly IValidator<RevealSecretRequest> _revealSecretValidator;

    public SecretService(
        IEncryptionService encryptionService,
        ISecretsRepository secretsRepository,
        IValidator<CreateSecretRequest> createSecretValidator,
        IValidator<RevealSecretRequest> revealSecretValidator
    )
    {
        _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
        _secretsRepository = secretsRepository ?? throw new ArgumentNullException(nameof(secretsRepository));
        _createSecretValidator = createSecretValidator ?? throw new ArgumentNullException(nameof(createSecretValidator));
        _revealSecretValidator = revealSecretValidator ?? throw new ArgumentNullException(nameof(revealSecretValidator));
    }

    public async Task<CreateSecretResponse> CreateSecret(CreateSecretRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        await _createSecretValidator.ValidateAndThrowAsync(request);

        string encryptedValue = _encryptionService.Encrypt(request.Secret, request.Passphrase);

        var secret = await _secretsRepository.CreateAsync(request.ToEntity(encryptedValue));

        return secret.ToCreateResponse();
    }

    public async Task<RevealSecretResponse> RevealAndDisposeSecret(RevealSecretRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

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

        return response;
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
