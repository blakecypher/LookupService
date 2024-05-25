using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// Configures the default services for dependency injection in the application.
/// </summary>
public static class WebApiDefaultConfig
{
    /// <summary>
    /// Configures the default services for dependency injection in the application.
    /// </summary>
    /// <param name="services"></param>
    public static void AddDefaultServices(
        this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            AddSwaggerDocument(c, @"swagger\v1\swagger.json");
            AddSwaggerDocument(c, @"swagger\v2\swagger.json");
            c.SupportNonNullableReferenceTypes();
            c.UseInlineDefinitionsForEnums();
            c.CustomSchemaIds(type => type.FullName);
            c.EnableAnnotations();
            c.UseAllOfToExtendReferenceSchemas();
        });

        services.AddHttpContextAccessor();

        services.Configure<JsonOptions>(o =>
        {
            o.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            o.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        services.AddControllers()
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

        services.ConfigureHttpJsonOptions(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
    }

    /// <summary>
    /// Configures the application's request pipeline.
    /// </summary>
    /// <param name="app"></param>
    public static void UseDefaultAppConfig(this IApplicationBuilder app)
    {
        app.UseDeveloperExceptionPage();
        app.UseStaticFiles();
        app.UseSwaggerUI(c =>
        {
            // For multiple API documentation
            c.SwaggerEndpoint("/swagger/v1/swagger.json","Lookup Service API");
            c.SwaggerEndpoint("/swagger/v2/swagger.json","Credit Data API");
        })
        .UseRouting()
        .UseRewriter(new RewriteOptions().AddRedirect("^$", "swagger"))
        .UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
    
    private static void AddSwaggerDocument(SwaggerGenOptions c, string path)
    {
        var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
        var json = File.ReadAllText(fullPath);
        var swaggerDoc = new OpenApiStringReader().Read(json, out _);
    
        c.SwaggerDoc(swaggerDoc.Info.Version, new OpenApiInfo
        {
            Title = swaggerDoc.Info.Title,
            Version = swaggerDoc.Info.Version
        });
    }
}