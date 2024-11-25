using System.Text;

namespace ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator;

public abstract record GenerateInfoBase(string Key);
public record JsonSourceGenerationInfo(
    string FullNamespace,
    string AccessModifier,
    string ClassName,
    string JsonSourceGenerationOptionsAttribute,
    string Key) : GenerateInfoBase(Key);

public record RegistrationToGenerateInfo(
    string FullNamespace, 
    string ClassName,
    string Key) : GenerateInfoBase(Key);

