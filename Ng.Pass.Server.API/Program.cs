using Azure.Identity;
using Ng.Pass.Server.API;
using Ng.Pass.Server.API.Helpers;

public static class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(
                (context, config) =>
                {
                    if (!EnvironmentVariableHelper.IsDevelopment())
                    {
                        string keyVaultUri = EnvironmentVariableHelper.GetKeyVaultUri();
                        config.AddAzureKeyVault(new Uri(keyVaultUri), new DefaultAzureCredential());
                    }
                }
            )
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
