using Microsoft.OpenApi.Models;

public class OpenApiDocumentModel
{
    public OpenApiInfo? Info { get; set; }
    public IList<OpenApiServer>? Servers { get; set; }
    public OpenApiPaths Paths { get; set; }
    public OpenApiComponents Components { get; set; }

    public OpenApiDocumentModel(OpenApiDocumentModel model)
    {
        model.Info = model.Info != null ? new OpenApiInfo(model.Info) : null;
        model.Servers = model.Servers != null ? new List<OpenApiServer>(model.Servers) : null;
        model.Paths = new OpenApiPaths(model.Paths);
        model.Components = new OpenApiComponents(model.Components);
    }

    public OpenApiDocumentModel()
    {
    }

}

public class OpenApiExampleModel
{
    public OpenApiExample? Example { get; set; }
    
}
