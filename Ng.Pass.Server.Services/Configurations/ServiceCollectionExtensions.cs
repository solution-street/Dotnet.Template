using EWA.CoreServices.Services.Users.Registration.Validators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ng.Pass.Server.DataLayer.Configurations;
using Ng.Pass.Server.DataLayer.Repositories.Secret;
using Ng.Pass.Server.DataLayer.Repositories.User;
using Ng.Pass.Server.Services.Encryption.Services;
using Ng.Pass.Server.Services.Secrets.Models;
using Ng.Pass.Server.Services.Secrets.Services;
using Ng.Pass.Server.Services.Shared.Services;

namespace Ng.Pass.Server.Services.Configurations;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Services
        services.AddScoped<ISecretService, SecretService>();
        services.AddScoped<IEncryptionService, EncryptionService>();
        services.AddScoped<IExecutorService, ExecutorService>();

        // Repositories
        services.AddScoped<ISecretRepository, SecretRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        // Validators
        services.AddScoped<IValidator<CreateSecretRequest>, CreateSecretRequestValidator>();
        services.AddScoped<IValidator<RevealSecretRequest>, RevealSecretRequestValidator>();

        // Database(s)
        services.AddDbContext(configuration);

        // External API Dependencies
        services.AddApiHttpClients();
        services.AddApiAuthenticationHandlers();
    }
}
