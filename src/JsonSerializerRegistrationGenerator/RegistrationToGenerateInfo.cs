using System.Collections.Immutable;
using System.Text;

namespace ProgrammerAL.JsonSerializerRegistrationGenerator;

public abstract record GenerateInfoBase(string Key);
public record JsonSourceGenerationInfo(
    string FullNamespace,
    string AccessModifier,
    string ClassName,
    string Key) : GenerateInfoBase(Key);

public record RegistrationToGenerateInfo(
    string FullNamespace,
    string ClassName,
    ImmutableArray<string> GenericTypeParameters,
    string Key) : GenerateInfoBase(Key)
{
    public string DetermineFullClassName()
    {
        if (GenericTypeParameters.Any())
        {
            return $"{ClassName}<{string.Join(", ", GenericTypeParameters)}>";
        }

        return ClassName;
    }
}

