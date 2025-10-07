using FluentValidation;
using Dotnet.Template.Server.Services.Secrets.Models;

namespace Dotnet.Template.Server.Services.Users.Registration.Validators;

public class RevealSecretRequestValidator : AbstractValidator<RevealSecretRequest>
{
    public RevealSecretRequestValidator()
    {
        RuleFor(x => x.Passphrase).NotEmpty().MinimumLength(3); // TODO: DRY
    }
}
