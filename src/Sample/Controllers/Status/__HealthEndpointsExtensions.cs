#pragma warning disable IDE0058 // Expression value is never used

namespace Sample.Controllers.Status;

public static class HealthEndpointsExtensions
{
    public static WebApplication MapHealthEndpoints(this WebApplication app)
    {
        var rootGroup = app.MapGroup("/");

        var tags = new[] { "Status" };

        RootCheckEndpoint.RegisterApiEndpoint(rootGroup).WithTags(tags);

        return app;
    }
}
#pragma warning restore IDE0058 // Expression value is never used
