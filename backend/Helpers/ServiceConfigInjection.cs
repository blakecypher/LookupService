using System.Net.Http;
using backend.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

/// <summary>
/// Fetches and caches the OpenApiDocument
/// </summary>
public class ServiceConfigInjection : IServiceConfigInjection
{
    private readonly HttpClient httpClientFactory;
    private readonly IGenericCache<OpenApiDocumentModel> openApiDocumentCache;
    private readonly string apiUrl;

    /// <summary>
    /// Fetches and caches the OpenApiDocument
    /// </summary>
    /// <param name="openApiDocumentCache"></param>
    /// <param name="configuration"></param>
    /// <param name="httpClientFactory"></param>
    public ServiceConfigInjection(
        IGenericCache<OpenApiDocumentModel> openApiDocumentCache,
        IConfiguration configuration,
        HttpClient httpClientFactory)
    {
        this.openApiDocumentCache = openApiDocumentCache;
        this.httpClientFactory = httpClientFactory;
        apiUrl = configuration["credit_data_url"];
    }

    /// <summary>
    /// Fetches and caches the OpenApiDocument
    /// </summary>
    /// <returns></returns>
    public OpenApiDocumentModel GetOrFetchOpenApiDocument()
    {
        return openApiDocumentCache.GetOrCreate("OpenApiDocument", GetOpenApiDocument);
    }

    /// <summary>
    /// Fetches the OpenApiDocument
    /// </summary>
    /// <returns></returns>
    public OpenApiDocumentModel GetOpenApiDocument()
    {
        var response = httpClientFactory.Send(new HttpRequestMessage(HttpMethod.Get, apiUrl));
        var content = response.Content.ReadAsStringAsync().Result;
        OpenApiDocument swaggerDocument = new OpenApiStringReader().Read(content, out _);
        var model = new OpenApiDocumentModel
        {
            Info = swaggerDocument.Info,
            Servers = swaggerDocument.Servers,
            Paths = swaggerDocument.Paths,
            Components = swaggerDocument.Components
        };
        return model;
    }
}
/// <summary>
/// Fetches and caches the OpenApiDocument
/// </summary>
public interface IServiceConfigInjection
{
    /// <summary>
    /// Fetches and caches the OpenApiDocument
    /// </summary>
    /// <returns></returns>
    public OpenApiDocumentModel GetOrFetchOpenApiDocument();
}