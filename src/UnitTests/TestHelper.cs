using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator;
using ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator.Attributes;
using System.Text.Json.Serialization;

namespace UnitTests;

public static class TestHelper
{
    public static async Task VerifyAsync(string source, string directory)
    {
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);
        // Create references for assemblies we require
        // We could add multiple references if required
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
            syntaxTrees: new[] { syntaxTree },
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
