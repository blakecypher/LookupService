using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using service;

/// <summary>
/// The main entry point of the application.
/// </summary>
/// <param name="configuration"></param>
public class Startup(IConfiguration configuration)
{
    /// <summary>
    /// Configures the services for dependency injection in the application.
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDefaultServices();
        services.AddHttpClient();
        services.AddMemoryCache();
        services.AddScoped<ILookupService, LookupService>();
        services.AddSingleton<IServiceConfigInjection, ServiceConfigInjection>();
        services.AddSingleton(_ => configuration);
    }

    /// <summary>
    /// Configures the application's request pipeline.
    /// </summary>
    /// <param name="app"></param>
    public void Configure(IApplicationBuilder app)
    {
        app.UseDefaultAppConfig();
    }
}