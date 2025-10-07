using Dotnet.Template.Server.API;
using Dotnet.Template.Server.API.Helpers;

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
                        // add secrets
                    }
                }
            )
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
