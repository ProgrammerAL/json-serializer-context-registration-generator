﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using ProgrammerAL.JsonSerializerRegistrationGenerator;
using ProgrammerAL.JsonSerializerRegistrationGenerator.Attributes;
using System.Text.Json.Serialization;

namespace UnitTests;

public static class TestHelper
{
    public static async Task VerifyAsync(string directory, params string[] sources)
    {
        var syntaxTrees = sources.Select(source => CSharpSyntaxTree.ParseText(source)).ToArray();

        // Create references for assemblies we require
        var references = AppDomain.CurrentDomain.GetAssemblies()
            .Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
            .Select(_ => MetadataReference.CreateFromFile(_.Location))
            .Concat(new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(JsonSerializerContextRegistrationGenerator).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(RegisterJsonSerializationAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(JsonSourceGenerationOptionsAttribute).Assembly.Location),
            });

        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: syntaxTrees,
            references: references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var driver = CSharpGeneratorDriver
                .Create(new JsonSerializerContextRegistrationGenerator())
                .RunGenerators(compilation);

        _ = await Verifier
                  .Verify(driver)
                  .UseDirectory(directory);
    }
}
