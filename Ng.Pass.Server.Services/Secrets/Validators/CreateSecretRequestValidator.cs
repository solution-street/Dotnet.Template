using FluentValidation;
using Ng.Pass.Server.Core.Enums;
using Ng.Pass.Server.Core.Extensions;
using Ng.Pass.Server.Services.Secrets.Models;

namespace EWA.CoreServices.Services.Users.Registration.Validators;

public class CreateSecretRequestValidator : AbstractValidator<CreateSecretRequest>
{
    public const string InvalidTimeToLiveValue = "The 'Ttl' field was not recognized as a valid TimeToLive value.";

    public CreateSecretRequestValidator()
    {
        RuleFor(x => x.Passphrase).NotEmpty().MinimumLength(3);

        RuleFor(x => x.Ttl)
            .Must((ttl) => Enum.GetValues<TimeToLive>().Select((x) => x.GetStringValue()).ToList().Contains(ttl))
            .WithMessage(InvalidTimeToLiveValue);
    }
}
