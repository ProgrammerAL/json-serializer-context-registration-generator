using System.Text;

using static ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator.RegistrationToGenerateInfo;

namespace ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator;
public record RegistrationToGenerateInfo(
    SerializerContextInfo SerializerContext,
    RegistrationInfo Registration)
{
    public string GenerateInterfaceDefinitionString()
    {
        var builder = new StringBuilder();

        _ = builder.AppendLine($"using {SerializerContext.FullNamespace};");
        _ = builder.AppendLine($"using {Registration.FullNamespace};");
        _ = builder.AppendLine($"using System.Text.Json.Serialization;");

        _ = builder.AppendLine($"");

        _ = builder.AppendLine($"[JsonSerializable(typeof({Registration.ClassName}))]");
        _ = builder.AppendLine($"internal partial {SerializerContext.ClassName} : JsonSerializerContext");
        _ = builder.AppendLine("{");
        _ = builder.AppendLine("}");

        return builder.ToString();
    }

    public record SerializerContextInfo(string FullNamespace, string ClassName);
    public record RegistrationInfo(string FullNamespace, string ClassName);
}
