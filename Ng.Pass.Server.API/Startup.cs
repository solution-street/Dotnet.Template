using EWA.Coordination.Common.Logic.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Ng.Pass.Server.API.Helpers;
using Ng.Pass.Server.API.Middleware;
using Ng.Pass.Server.Core.Configuration;
using Ng.Pass.Server.Database.Contexts;
using Ng.Pass.Server.Services.Configurations;

namespace Ng.Pass.Server.API;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        string[] allowedOrigins = EnvironmentVariableHelper.GetClientOriginUrls();

        services.AddCors(options =>
        {
            options.AddPolicy(
                "AllowSpecificOrigin",
                builder =>
                {
                    builder.WithOrigins(allowedOrigins).AllowAnyMethod().AllowAnyHeader();
                }
            );
        });

        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // This is the default authentication scheme for the application. This handles user-based authentication.
            .AddJwtBearer(
                JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.Authority = EnvironmentVariableHelper.GetAuthCodeBearerTokenAuthority();
                    options.Audience = EnvironmentVariableHelper.GetAuthCodeBearerTokenAudience();
                }
            );

        //services.AddAuthorizationPoliciesAndHandlers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Cevo API",
                    Description = "API for Cevo Application"
                }
            );
            // To Enable authorization using Swagger (JWT)
            c.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme()
                {
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme.ToLower(),
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter the bearer token in the text input below.\r\n\r\nExample: \"12345abcdef\"",
                }
            );
            c.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new string[] { }
                    }
                }
            );
        });

        if (!EnvironmentVariableHelper.IsDevelopment())
        {
            services.AddApplicationInsightsTelemetry();

            //services.AddLogging(builder =>
            //{
            //    builder.AddApplicationInsights(
            //        configureTelemetryConfiguration: (config) =>
            //            config.ConnectionString = _configuration.GetRequiredValue(
            //                AppConstants.Secrets.ApplicationInsightsConnectionStringKey
            //            ),
            //        configureApplicationInsightsLoggerOptions: (options) => { }
            //    );
            //});
        }

        RegisterServices(services);

        services.AddControllers(options =>
        {
            options.ModelMetadataDetailsProviders.Add(new IgnoreValidationMetadataProvider());
        });
        services.AddEndpointsApiExplorer();
    }

    /// <summary>
    /// Registers all services for the application.
    /// </summary>
    /// <param name="services"></param>
    private void RegisterServices(IServiceCollection services)
    {
        // API project service registrations

        // Filters

        services.AddHttpClient();

        // Service layer service registrations
        services.AddServices(_configuration);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        ConfigureRouting(app);
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        });

        // Apply any pending migrations
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<NgPassContext>();

            if (db.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                db.Database.Migrate();
            }
        }
    }

    private static void ConfigureRouting(IApplicationBuilder app)
    {
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("AllowSpecificOrigin");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
