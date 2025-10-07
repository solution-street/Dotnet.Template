using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Dotnet.Template.Server.DataLayer.Configurations;
using Dotnet.Template.Server.DataLayer.Repositories.Secret;
using Dotnet.Template.Server.DataLayer.Repositories.User;
using Dotnet.Template.Server.Services.Encryption.Services;
using Dotnet.Template.Server.Services.Secrets.Models;
using Dotnet.Template.Server.Services.Secrets.Services;
using Dotnet.Template.Server.Services.Shared.Services;
using Dotnet.Template.Server.Services.Users.Registration.Validators;

namespace Dotnet.Template.Server.Services.Configurations;

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
