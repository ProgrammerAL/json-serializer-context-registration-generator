//HintName: MyAppJsonSerializerContext.Registrations.g.cs
using System.Text.Json.Serialization;

namespace ProgrammerAL.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

[JsonSerializable(typeof(MyClass))]
[JsonSerializable(typeof(MyRecord))]
public partial class MyAppJsonSerializerContext : JsonSerializerContext
{
}
