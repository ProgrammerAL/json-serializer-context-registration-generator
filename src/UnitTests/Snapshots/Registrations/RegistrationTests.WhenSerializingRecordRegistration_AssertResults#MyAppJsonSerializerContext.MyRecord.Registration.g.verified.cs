//HintName: MyAppJsonSerializerContext.MyRecord.Registration.g.cs
using UnitTestClasses;
using ProgrammerAL.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;
using System.Text.Json.Serialization;

[JsonSerializable(typeof(MyRecord))]
internal partial MyAppJsonSerializerContext : JsonSerializerContext
{
}
