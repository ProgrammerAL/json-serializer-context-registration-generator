#pragma warning disable IDE0058 // Expression value is never used

using Sample.Controllers.Status;

namespace Sample;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
            //options.SerializerOptions.TypeInfoResolverChain.Insert(0, MyOtherAppJsonSerializerContext.Default);
            //MyOtherAppJsonSerializerContext

            //options.SerializerOptions.TypeInfoResolverChain.Insert(0, MyGeneratedJsonSerializerContext.Default);
        });

        // Add services to the container.
        builder.Services.AddAuthorization();

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapHealthEndpoints();

        app.Run();
    }
}
#pragma warning restore IDE0058 // Expression value is never used
