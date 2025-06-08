using FluentValidation;
using Ng.Pass.Server.Services.Passwords.Models;

namespace EWA.CoreServices.Services.Users.Registration.Validators;

public class RevealPasswordRequestValidator : AbstractValidator<RevealPasswordRequest>
{
    public RevealPasswordRequestValidator()
    {
        RuleFor(x => x.Passphrase).NotEmpty().MinimumLength(3); // TODO: DRY
    }
}
