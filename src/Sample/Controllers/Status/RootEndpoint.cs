using System.Net;
using System.Threading.Tasks;

using ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator.Attributes;

namespace Sample.Controllers.Status;

public class RootCheckEndpoint
{
    public static RouteHandlerBuilder RegisterApiEndpoint(RouteGroupBuilder group)
    {
        return group.MapGet("/", () =>
        {
            var response = new RootCheckEndpointResponse(DateTime.UtcNow.ToString());
            return TypedResults.Ok(response);
        })
        .WithSummary($"Simple return from the root endpoint");
    }
}

[RegisterJsonSerialization(typeof(AppJsonSerializerContext))]
public record RootCheckEndpointResponse(string UtcTime);
