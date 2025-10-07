using FluentValidation;
using Dotnet.Template.Server.Core.Enums;
using Dotnet.Template.Server.Core.Extensions;
using Dotnet.Template.Server.Services.Secrets.Models;

namespace Dotnet.Template.Server.Services.Users.Registration.Validators;

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
