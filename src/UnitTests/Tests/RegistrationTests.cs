namespace UnitTests.Tests;

public class RegistrationTests
{
    private const string SnapshotsDirectory = "Snapshots/Registrations";

    [Fact]
    public async Task WhenSerializingClassRegistration_AssertResults()
    {
        var source = """
            using ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator.Attributes;
            namespace ProgrammerAL.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [RegisterJsonSerialization(typeof(MyAppJsonSerializerContext))]
            public class MyClass
            {
                public string MyProp { get; set; }
            }

            [JsonSourceGenerationOptions()]
            internal partial class MyAppJsonSerializerContext : JsonSerializerContext
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task WhenSerializingRecordRegistration_AssertResults()
    {
        var source = """
            using ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator.Attributes;
            namespace ProgrammerAL.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [RegisterJsonSerialization(typeof(MyAppJsonSerializerContext))]
            public record MyRecord(string MyProp);

            [JsonSourceGenerationOptions()]
            internal partial class MyAppJsonSerializerContext : JsonSerializerContext
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }
}
