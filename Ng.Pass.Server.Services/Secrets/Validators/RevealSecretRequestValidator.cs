using FluentValidation;
using Ng.Pass.Server.Services.Secrets.Models;

namespace Ng.Pass.Server.Services.Users.Registration.Validators;

public class RevealSecretRequestValidator : AbstractValidator<RevealSecretRequest>
{
    public RevealSecretRequestValidator()
    {
        RuleFor(x => x.Passphrase).NotEmpty().MinimumLength(3); // TODO: DRY
    }
}
