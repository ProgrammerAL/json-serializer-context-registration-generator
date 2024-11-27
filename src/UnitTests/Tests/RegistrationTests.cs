namespace UnitTests.Tests;

public class RegistrationTests
{
    private const string SnapshotsDirectory = "Snapshots/Registrations";

    [Fact]
    public async Task WhenSerializingPublicClassRegistration_AssertResults()
    {
        var source = """
            using System.Text.Json.Serialization;
            using ProgrammerAL.JsonSerializerRegistrationGenerator.Attributes;
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
            public partial class MyAppJsonSerializerContext: JsonSerializerContext
            {
            }
            """;

        await TestHelper.VerifyAsync(SnapshotsDirectory, source);
    }

    [Fact]
    public async Task WhenSerializingGenericClassRegistration_AssertResults()
    {
        var source = """
            using System.Text.Json.Serialization;
            using ProgrammerAL.JsonSerializerRegistrationGenerator.Attributes;
            namespace ProgrammerAL.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [RegisterJsonSerialization(typeof(MyAppJsonSerializerContext))]
            public class MyClass<T>
            {
                public T MyProp { get; set; }
            }

            [JsonSourceGenerationOptions(
                UseStringEnumConverter = true, 
                PropertyNameCaseInsensitive = true, 
                PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
                AllowTrailingCommas = true)]
            [GeneratedJsonSerializerContext]
            public partial class MyAppJsonSerializerContext: JsonSerializerContext
            {
            }
            """;

        await TestHelper.VerifyAsync(SnapshotsDirectory, source);
    }

    [Fact]
    public async Task WhenSerializingInternalClassRegistration_AssertResults()
    {
        var source = """
            using System.Text.Json.Serialization;
            using ProgrammerAL.JsonSerializerRegistrationGenerator.Attributes;
            namespace ProgrammerAL.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [JsonSourceGenerationOptions(
                UseStringEnumConverter = true, 
                PropertyNameCaseInsensitive = true, 
                PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
                AllowTrailingCommas = true)]
            [GeneratedJsonSerializerContext]
            internal partial class MyAppJsonSerializerContext: JsonSerializerContext
            {
            }

            [RegisterJsonSerialization(typeof(MyAppJsonSerializerContext))]
            public class MyClass
            {
                public string MyProp { get; set; }
            }
            """;

        await TestHelper.VerifyAsync(SnapshotsDirectory, source);
    }

    [Fact]
    public async Task WhenSerializingPrivateClassRegistration_AssertResults()
    {
        var source = """
            using System.Text.Json.Serialization;
            using ProgrammerAL.JsonSerializerRegistrationGenerator.Attributes;
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
            private partial class MyAppJsonSerializerContext: JsonSerializerContext
            {
            }
            """;

        await TestHelper.VerifyAsync(SnapshotsDirectory, source);
    }

    [Fact]
    public async Task WhenSerializingRegistrationsInDifferentFiles_AssertResults()
    {
        var source1 = """
            using System.Text.Json.Serialization;
            using ProgrammerAL.JsonSerializerRegistrationGenerator.Attributes;
            namespace ProgrammerAL.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [RegisterJsonSerialization(typeof(MyAppJsonSerializerContext))]
            public class MyClass
            {
                public string MyProp { get; set; }
            }
            """;

        var source2 = """
            using System.Text.Json.Serialization;
            using ProgrammerAL.JsonSerializerRegistrationGenerator.Attributes;
            namespace ProgrammerAL.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [JsonSourceGenerationOptions(
                UseStringEnumConverter = true, 
                PropertyNameCaseInsensitive = true, 
                PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
                AllowTrailingCommas = true)]
            [GeneratedJsonSerializerContext]
            public partial class MyAppJsonSerializerContext: JsonSerializerContext
            {
            }
            """;

        await TestHelper.VerifyAsync(SnapshotsDirectory, source1, source2);
    }


    [Fact]
    public async Task WhenRegisteringClassAndRecord_AssertResults()
    {
        var source1 = """
            using System.Text.Json.Serialization;
            using ProgrammerAL.JsonSerializerRegistrationGenerator.Attributes;
            namespace ProgrammerAL.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [RegisterJsonSerialization(typeof(MyAppJsonSerializerContext))]
            public class MyClass
            {
                public string MyProp { get; set; }
            }
            """;

        var source2 = """
            using System.Text.Json.Serialization;
            using ProgrammerAL.JsonSerializerRegistrationGenerator.Attributes;
            namespace ProgrammerAL.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [RegisterJsonSerialization(typeof(MyAppJsonSerializerContext))]
            public record MyRecord(string MyProp);
            """;

        var source3 = """
            using System.Text.Json.Serialization;
            using ProgrammerAL.JsonSerializerRegistrationGenerator.Attributes;
            namespace ProgrammerAL.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [JsonSourceGenerationOptions(
                UseStringEnumConverter = true, 
                PropertyNameCaseInsensitive = true, 
                PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
                AllowTrailingCommas = true)]
            [GeneratedJsonSerializerContext]
            public partial class MyAppJsonSerializerContext: JsonSerializerContext
            {
            }
            """;
        await TestHelper.VerifyAsync(SnapshotsDirectory, source1, source2, source3);
    }
}
