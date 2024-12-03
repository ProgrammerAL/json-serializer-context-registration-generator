using System.Net;
using System.Threading.Tasks;

using ProgrammerAL.JsonSerializerRegistrationGenerator.Attributes;

namespace Sample.Controllers.Status;

public class MultipleGenericRootEndpoint
{
    public static RouteHandlerBuilder RegisterApiEndpoint(RouteGroupBuilder group)
    {
        return group.MapGet("/", () =>
        {
            var response = new MultipleGenericRootCheckEndpointResponse<string, int>(DateTime.UtcNow.ToString(), 123);
            return TypedResults.Ok(response);
        })
        .WithSummary($"Simple return from the root endpoint");
    }
}

[RegisterJsonSerialization(typeof(AppJsonSerializerContext), genericTypes: ["string, int", "int, DateTime"])]
public record MultipleGenericRootCheckEndpointResponse<T, U>(T UtcTime, U Other);
