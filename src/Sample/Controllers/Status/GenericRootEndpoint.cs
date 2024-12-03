using System.Net;
using System.Threading.Tasks;

using ProgrammerAL.JsonSerializerRegistrationGenerator.Attributes;

namespace Sample.Controllers.Status;

public class GenericRootCheckEndpoint
{
    public static RouteHandlerBuilder RegisterApiEndpoint(RouteGroupBuilder group)
    {
        return group.MapGet("/", () =>
        {
            var response = new GenericRootCheckEndpointResponse<string>(DateTime.UtcNow.ToString());
            return TypedResults.Ok(response);
        })
        .WithSummary($"Simple return from the root endpoint");
    }
}

[RegisterJsonSerialization(typeof(AppJsonSerializerContext), genericTypes: ["string", "int", "System.DateTime"])]
public record GenericRootCheckEndpointResponse<T>(T UtcTime);
