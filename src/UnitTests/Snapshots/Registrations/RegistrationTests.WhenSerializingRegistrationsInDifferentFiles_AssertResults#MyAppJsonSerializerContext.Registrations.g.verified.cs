//HintName: MyAppJsonSerializerContext.Registrations.g.cs
using System.Text.Json.Serialization;

namespace ProgrammerAL.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

[JsonSerializable(typeof(MyClass))]
[System.Text.Json.Serialization.JsonSourceGenerationOptionsAttribute(UseStringEnumConverter = true, PropertyNameCaseInsensitive = true, PropertyNamingPolicy = System.Text.Json.Serialization.JsonKnownNamingPolicy.CamelCase, AllowTrailingCommas = true)]
public partial class MyAppJsonSerializerContext : JsonSerializerContext
{
}
