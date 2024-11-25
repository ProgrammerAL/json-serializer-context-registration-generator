using System.Text.Json.Serialization;

using ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator.Attributes;


namespace Sample;

[GeneratedJsonSerializerContext]
[JsonSourceGenerationOptions(
    UseStringEnumConverter = true,
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    AllowTrailingCommas = true)]
internal class AppJsonSerializerContext
{
}

////[JsonSourceGenerationOptions(
////    UseStringEnumConverter = true,
////    PropertyNameCaseInsensitive = true,
////    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
////    AllowTrailingCommas = true)]
//[GeneratedJsonSerializerContext]
//public partial class MyOtherAppJsonSerializerContext
//{
//}

//[JsonSerializable(typeof(RootCheckEndpointResponse))]
//[System.Text.Json.Serialization.JsonSourceGenerationOptionsAttribute(UseStringEnumConverter = true, PropertyNameCaseInsensitive = true, PropertyNamingPolicy = System.Text.Json.Serialization.JsonKnownNamingPolicy.CamelCase, AllowTrailingCommas = true)]
//internal partial class MyOtherAppJsonSerializerContext : JsonSerializerContext
//{
//}

//[JsonSerializable(typeof(RootCheckEndpointResponse))]
//[System.Text.Json.Serialization.JsonSourceGenerationOptionsAttribute(UseStringEnumConverter = true, PropertyNameCaseInsensitive = true, PropertyNamingPolicy = System.Text.Json.Serialization.JsonKnownNamingPolicy.CamelCase, AllowTrailingCommas = true)]
//internal partial class MyGeneratedJsonSerializerContext : JsonSerializerContext
//{
//}
