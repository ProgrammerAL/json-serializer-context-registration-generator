namespace UnitTests.Tests;

public class RegistrationTests
{
    private const string SnapshotsDirectory = "Snapshots/Registrations";

    [Fact]
    public async Task WhenSerializingPublicClassRegistration_AssertResults()
    {
        var source = """
            using System.Text.Json.Serialization;
            using ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator.Attributes;
            namespace ProgrammerAL.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [RegisterJsonSerialization(typeof(MyAppJsonSerializerContext))]
            public class MyClass
            {
                public string MyProp { get; set; }
            }

            [JsonSourceGenerationOptions(
                UseStringEnumConverter = true, 
                PropertyNameCaseInsensitive = true, 
                PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
                AllowTrailingCommas = true)]
            [GeneratedJsonSerializerContext]
            public partial class MyAppJsonSerializerContext
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task WhenSerializingInternalClassRegistration_AssertResults()
    {
        var source = """
            using System.Text.Json.Serialization;
            using ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator.Attributes;
            namespace ProgrammerAL.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [RegisterJsonSerialization(typeof(MyAppJsonSerializerContext))]
            public class MyClass
            {
                public string MyProp { get; set; }
            }

            [JsonSourceGenerationOptions(
                UseStringEnumConverter = true, 
                PropertyNameCaseInsensitive = true, 
                PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
                AllowTrailingCommas = true)]
            [GeneratedJsonSerializerContext]
            internal partial class MyAppJsonSerializerContext : JsonSerializerContext
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task WhenSerializingPrivateClassRegistration_AssertResults()
    {
        var source = """
            using System.Text.Json.Serialization;
            using ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator.Attributes;
            namespace ProgrammerAL.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [RegisterJsonSerialization(typeof(MyAppJsonSerializerContext))]
            public class MyClass
            {
                public string MyProp { get; set; }
            }

            [JsonSourceGenerationOptions(
                UseStringEnumConverter = true, 
                PropertyNameCaseInsensitive = true, 
                PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
                AllowTrailingCommas = true)]
            [GeneratedJsonSerializerContext]
            private partial class MyAppJsonSerializerContext : JsonSerializerContext
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task WhenSerializingRecordRegistration_AssertResults()
    {
        var source = """
            using System.Text.Json.Serialization;
            using ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator.Attributes;
            namespace ProgrammerAL.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [RegisterJsonSerialization(typeof(MyAppJsonSerializerContext))]
            public record MyRecord(string MyProp);

            [JsonSourceGenerationOptions(
                UseStringEnumConverter = true, 
                PropertyNameCaseInsensitive = true, 
                PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
                AllowTrailingCommas = true)]
            [GeneratedJsonSerializerContext]
            internal partial class MyAppJsonSerializerContext : JsonSerializerContext
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }
}
