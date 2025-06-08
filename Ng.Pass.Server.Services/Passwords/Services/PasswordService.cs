using FluentValidation;
using Ng.Pass.Server.Core.Enums;
using Ng.Pass.Server.Core.Exceptions;
using Ng.Pass.Server.Core.Extensions;
using Ng.Pass.Server.Database.Entities;
using Ng.Pass.Server.DataLayer.Repositories;
using Ng.Pass.Server.Services.Encryption.Services;
using Ng.Pass.Server.Services.Passwords.MappingProfiles;
using Ng.Pass.Server.Services.Passwords.Models;

namespace Ng.Pass.Server.Services.Passwords.Services;

public class PasswordService : IPasswordService
{
    private readonly IEncryptionService _encryptionService;

    private readonly IPasswordsRepository _passwordsRepository;

    private readonly IValidator<CreatePasswordRequest> _createPasswordValidator;
    private readonly IValidator<RevealPasswordRequest> _revealPasswordValidator;

    public PasswordService(
        IEncryptionService encryptionService,
        IPasswordsRepository passwordsRepository,
        IValidator<CreatePasswordRequest> createPasswordValidator,
        IValidator<RevealPasswordRequest> revealPasswordValidator
    )
    {
        _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
        _passwordsRepository = passwordsRepository ?? throw new ArgumentNullException(nameof(passwordsRepository));
        _createPasswordValidator = createPasswordValidator ?? throw new ArgumentNullException(nameof(createPasswordValidator));
        _revealPasswordValidator = revealPasswordValidator ?? throw new ArgumentNullException(nameof(revealPasswordValidator));
    }

    public async Task<CreatePasswordResponse> CreatePassword(CreatePasswordRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        await _createPasswordValidator.ValidateAndThrowAsync(request);

        string encryptedValue = _encryptionService.Encrypt(request.Secret, request.Passphrase);

        var password = await _passwordsRepository.CreateAsync(request.ToEntity(encryptedValue));

        return password.ToCreateResponse();
    }

    public async Task<RevealPasswordResponse> RevealAndDestroyPassword(RevealPasswordRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var password = await _passwordsRepository.TryGetByGuidAsync(request.Guid);

        if (password == null)
        {
            throw new KeyNotFoundException($"Password with GUID {request.Guid} not found.");
        }

        var latestTime = GetAtTheLatest(password);
        if (DateTime.UtcNow > latestTime)
        {
            throw new ApplicationLogicException(
                $"The password with GUID {request.Guid} has expired. It was created at {password.CreatedAt} and is valid until {latestTime}."
            );
        }

        var decryptedValue = _encryptionService.Decrypt(password.Value, request.Passphrase);

        var response = password.ToRevealResponse(decryptedValue);

        await _passwordsRepository.DeleteAsync(password);

        return response;
    }

    private DateTime GetAtTheLatest(Password password)
    {
        var timeToLive = password.Ttl;

        if (string.Equals(timeToLive, TimeToLive.Hour.GetStringValue()))
        {
            return password.CreatedAt.AddHours(1);
        }
        if (string.Equals(timeToLive, TimeToLive.Day.GetStringValue()))
        {
            return password.CreatedAt.AddDays(1);
        }
        if (string.Equals(timeToLive, TimeToLive.Week.GetStringValue()))
        {
            return password.CreatedAt.AddHours(1);
        }

        throw new ApplicationLogicException($"The 'Ttl' field was not recognized as a valid TimeToLive value: {timeToLive}.");
    }
}
