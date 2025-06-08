using FluentValidation;
using Ng.Pass.Server.Core.Enums;
using Ng.Pass.Server.Core.Extensions;
using Ng.Pass.Server.Services.Passwords.Models;

namespace EWA.CoreServices.Services.Users.Registration.Validators;

public class CreatePasswordRequestValidator : AbstractValidator<CreatePasswordRequest>
{
    public const string InvalidTimeToLiveValue = "The 'Ttl' field was not recognized as a valid TimeToLive value.";

    public CreatePasswordRequestValidator()
    {
        RuleFor(x => x.Passphrase).NotEmpty().MinimumLength(3);

        RuleFor(x => x.Ttl)
            .Must((ttl) => Enum.GetValues<TimeToLive>().Select((x) => x.GetStringValue()).ToList().Contains(ttl))
            .WithMessage(InvalidTimeToLiveValue);
    }
}
