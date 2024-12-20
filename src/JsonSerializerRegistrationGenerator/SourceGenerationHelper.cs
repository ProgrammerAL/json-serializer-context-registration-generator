﻿#pragma warning disable IDE0058 // Expression value is never used
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace ProgrammerAL.JsonSerializerRegistrationGenerator;

public static class SourceGenerationHelper
{
    public static string GenerateSource(JsonSourceGenerationInfo jsonSourceGenerationInfo, ImmutableArray<RegistrationToGenerateInfo> registrationInfos)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"//------------------------------------------------------------------------------");
        builder.AppendLine($"// <auto-generated>");
        builder.AppendLine($"//     This code was generated by a tool.");
        builder.AppendLine($"//");
        builder.AppendLine($"//     Changes to this file may cause incorrect behavior and will be lost if");
        builder.AppendLine($"//     the code is regenerated.");
        builder.AppendLine($"// </auto-generated>");
        builder.AppendLine($"//------------------------------------------------------------------------------");
        builder.AppendLine($"");

        var usingNamespaces = registrationInfos.Select(x => x.FullNamespace)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();

        usingNamespaces.Add("System.Text.Json.Serialization");

        var distinctUsingNamespaces = usingNamespaces.Distinct().ToList();

        //This will be in the namespace of the generated class, so don't add it to the usings
        distinctUsingNamespaces.Remove(jsonSourceGenerationInfo.FullNamespace);

        foreach (var usingNamespace in distinctUsingNamespaces)
        {
            builder.AppendLine($"using {usingNamespace};");
        }

        builder.AppendLine($"");
        builder.AppendLine($"namespace {jsonSourceGenerationInfo.FullNamespace};");

        builder.AppendLine($"");

        var registrationClassNames = registrationInfos.SelectMany(x => x.DetermineClassNames()).OrderBy(x => x);

        foreach (var registrationClassName in registrationClassNames)
        {
            builder.AppendLine($"[JsonSerializable(typeof({registrationClassName}))]");
        }

        builder.AppendLine($"{jsonSourceGenerationInfo.AccessModifier} partial class {jsonSourceGenerationInfo.ClassName} : JsonSerializerContext");
        builder.AppendLine("{");
        builder.AppendLine("}");

        return builder.ToString();
    }
}
#pragma warning restore IDE0058 // Expression value is never used
