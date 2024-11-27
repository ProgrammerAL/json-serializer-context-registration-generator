using System.Text.Json.Serialization;

using ProgrammerAL.JsonSerializerRegistrationGenerator.Attributes;

namespace Sample;

[GeneratedJsonSerializerContext]
[JsonSourceGenerationOptions(
    UseStringEnumConverter = true,
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    AllowTrailingCommas = true)]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}

