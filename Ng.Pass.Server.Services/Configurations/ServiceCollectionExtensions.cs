using EWA.CoreServices.Services.Users.Registration.Validators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ng.Pass.Server.DataLayer.Configurations;
using Ng.Pass.Server.DataLayer.Repositories;
using Ng.Pass.Server.Services.Encryption.Services;
using Ng.Pass.Server.Services.Passwords.Models;
using Ng.Pass.Server.Services.Passwords.Services;

namespace Ng.Pass.Server.Services.Configurations;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Services
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IEncryptionService, EncryptionService>();

        // Repositories
        services.AddScoped<IPasswordsRepository, PasswordsRepository>();

        // Validators
        services.AddScoped<IValidator<CreatePasswordRequest>, CreatePasswordRequestValidator>();
        services.AddScoped<IValidator<RevealPasswordRequest>, RevealPasswordRequestValidator>();

        // Database(s)
        services.AddDbContext(configuration);

        // External API Dependencies
        services.AddApiHttpClients();
        services.AddApiAuthenticationHandlers();
    }
}
