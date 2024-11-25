//HintName: MyAppJsonSerializerContext.Registrations.g.cs
using System.Text.Json.Serialization;

namespace ProgrammerAL.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

[JsonSerializable(typeof(MyClass))]
internal partial class MyAppJsonSerializerContext : JsonSerializerContext
{
}
