﻿#pragma warning disable IDE0058 // Expression value is never used
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator;

public static class SourceGenerationHelper
{
    public static string GenerateSource(JsonSourceGenerationInfo jsonSourceGenerationInfo, ImmutableArray<RegistrationToGenerateInfo> registrationInfos)
    {
        var builder = new StringBuilder();

        var usingNamespaces = registrationInfos.Select(x => x.FullNamespace)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();

        usingNamespaces.Add("System.Text.Json.Serialization");
        if (!string.IsNullOrWhiteSpace(jsonSourceGenerationInfo.FullNamespace))
        {
            usingNamespaces.Add(jsonSourceGenerationInfo.FullNamespace);
        }

        var distinctUsingNamespaces = usingNamespaces.Distinct();
        foreach (var usingNamespace in distinctUsingNamespaces)
        {
            builder.AppendLine($"using {usingNamespace};");
        }

        builder.AppendLine($"");

        var reistrationClassNames = registrationInfos.Select(x => x.ClassName).OrderBy(x => x);

        foreach (var reistrationClassName in reistrationClassNames)
        {
            builder.AppendLine($"[JsonSerializable(typeof({reistrationClassName}))]");
        }


        builder.AppendLine($"[{jsonSourceGenerationInfo.JsonSourceGenerationOptionsAttribute}]");
        builder.AppendLine($"{jsonSourceGenerationInfo.AccessModifier} partial class {jsonSourceGenerationInfo.ClassName} : JsonSerializerContext");
        builder.AppendLine("{");
        builder.AppendLine("}");

        return builder.ToString();
    }
}
#pragma warning restore IDE0058 // Expression value is never used