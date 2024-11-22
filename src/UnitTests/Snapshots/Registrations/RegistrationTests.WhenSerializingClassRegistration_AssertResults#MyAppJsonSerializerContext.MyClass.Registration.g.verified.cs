//HintName: MyAppJsonSerializerContext.MyClass.Registration.g.cs
using UnitTestClasses;
using ProgrammerAL.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;
using System.Text.Json.Serialization;

[JsonSerializable(typeof(MyClass))]
internal partial MyAppJsonSerializerContext : JsonSerializerContext
{
}
