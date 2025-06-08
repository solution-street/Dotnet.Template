using FluentValidation;
using Ng.Pass.Server.Services.Secrets.Models;

namespace EWA.CoreServices.Services.Users.Registration.Validators;

public class RevealSecretRequestValidator : AbstractValidator<RevealSecretRequest>
{
    public RevealSecretRequestValidator()
    {
        RuleFor(x => x.Passphrase).NotEmpty().MinimumLength(3); // TODO: DRY
    }
}
