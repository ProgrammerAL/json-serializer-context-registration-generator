namespace UnitTests.Tests;

public class RegistrationTests
{
    private const string SnapshotsDirectory = "Snapshots/Registrations";

    [Fact]
    public async Task WhenSerializingPublicClassRegistration_AssertResults()
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
            public partial class MyAppJsonSerializerContext : JsonSerializerContext
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task WhenSerializingInternalClassRegistration_AssertResults()
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
    public async Task WhenSerializingPrivateClassRegistration_AssertResults()
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

    [Fact]
    public async Task WhenSerializingClassRegistrationInOtherFile_AssertResults()
    {
        var source = """
            using ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator.Attributes;
            namespace ProgrammerAL.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [RegisterJsonSerialization(typeof(MyAppJsonSerializerContext))]
            public class MyClass
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task __AAA()
    {
        var source = """
            using ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator.Attributes;
            namespace ProgrammerAL.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [RegisterJsonSerialization(typeof(MyAppJsonSerializerContext))]
            public class MyClass_1
            {
            }

            [RegisterJsonSerialization(typeof(MyAppJsonSerializerContext))]
            public class MyClass_2
            {
            }

            [RegisterJsonSerialization(typeof(MyAppJsonSerializerContext))]
            public class MyClass_3
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }
}
