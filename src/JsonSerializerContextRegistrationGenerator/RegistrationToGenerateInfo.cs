using System.Text;

using static ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator.RegistrationToGenerateInfo;

namespace ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator;

public abstract record GenerateInfoBase();
public record JsonSourceGenerationInfo() : GenerateInfoBase;

public record RegistrationToGenerateInfo(
    SerializerContextInfo SerializerContext,
    RegistrationInfo Registration) : GenerateInfoBase
{
    public string GenerateInterfaceDefinitionString()
    {
        var builder = new StringBuilder();

        if (!string.IsNullOrWhiteSpace(SerializerContext.FullNamespace))
        {
            _ = builder.AppendLine($"using {SerializerContext.FullNamespace};");
        }

        if (!string.IsNullOrWhiteSpace(Registration.FullNamespace))
        {
            _ = builder.AppendLine($"using {Registration.FullNamespace};");
        }
        _ = builder.AppendLine($"using System.Text.Json.Serialization;");

        _ = builder.AppendLine($"");

        _ = builder.AppendLine($"[JsonSerializable(typeof({Registration.ClassName}))]");
        _ = builder.AppendLine($"{SerializerContext.AccessModifier} partial class {SerializerContext.ClassName} : JsonSerializerContext");
        _ = builder.AppendLine("{");
        _ = builder.AppendLine("}");

        return builder.ToString();
    }

    public record SerializerContextInfo(string FullNamespace, string AccessModifier, string ClassName);
    public record RegistrationInfo(string FullNamespace, string ClassName);
}
