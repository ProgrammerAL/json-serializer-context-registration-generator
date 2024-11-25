using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator.Attributes;

using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator.Runner;

class Program
{
    static void Main(string[] args)
    {
        var sourceFiles = Directory.GetFiles(@"D:\GitHub\ProgrammerAL\json-serializer-context-registration-generator\src\Sample", "*.cs", SearchOption.AllDirectories);
        var outDir = @"D:\GitHub\ProgrammerAL\json-serializer-context-registration-generator\src\Sample\Generated";

        var sources = sourceFiles.Select(File.ReadAllText).ToImmutableArray();
        var syntaxTrees = sources.Select(source => CSharpSyntaxTree.ParseText(source)).ToImmutableArray();

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

        var compilation = CSharpCompilation.Create(
            assemblyName: "GeneratedCodeFiles",
            syntaxTrees: syntaxTrees,
            references: references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var driver = CSharpGeneratorDriver
                .Create(new JsonSerializerContextRegistrationGenerator())
                .RunGenerators(compilation);

        var generatedSources = driver.GetRunResult()
                                        .Results
                                        .SelectMany(x => x.GeneratedSources)
                                        .ToImmutableArray();

        if (!Directory.Exists(outDir))
        {
            _ = Directory.CreateDirectory(outDir);
        }

        foreach (var generatedSource in generatedSources)
        {
            var outPath = $@"{outDir}\{generatedSource.HintName}";
            File.WriteAllText(outPath, generatedSource.SourceText.ToString());
        }
    }
}
