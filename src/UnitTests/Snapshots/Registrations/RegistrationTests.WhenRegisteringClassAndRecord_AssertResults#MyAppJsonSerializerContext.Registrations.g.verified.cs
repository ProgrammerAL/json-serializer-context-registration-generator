//HintName: MyAppJsonSerializerContext.Registrations.g.cs
using ProgrammerAL.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;
using System.Text.Json.Serialization;

[JsonSerializable(typeof(MyClass))]
[JsonSerializable(typeof(MyRecord))]
[System.Text.Json.Serialization.JsonSourceGenerationOptionsAttribute(UseStringEnumConverter = true, PropertyNameCaseInsensitive = true, PropertyNamingPolicy = System.Text.Json.Serialization.JsonKnownNamingPolicy.CamelCase, AllowTrailingCommas = true)]
public partial class MyAppJsonSerializerContext : JsonSerializerContext
{
}
