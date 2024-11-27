using System.Text;

using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using ProgrammerAL.JsonSerializerRegistrationGenerator.Attributes;
using ProgrammerAL.JsonSerializerRegistrationGenerator.GeneratorParsers;
using System.Collections.Immutable;
using System.Diagnostics;

namespace ProgrammerAL.JsonSerializerRegistrationGenerator;

[Generator]
public class JsonSerializerContextRegistrationGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var registrations =
            context.SyntaxProvider
            .ForAttributeWithMetadataName(
                RegisterJsonSerializationAttribute.Constants.AttributeFullName,
                predicate: static (node, _) => node is ClassDeclarationSyntax or RecordDeclarationSyntax,
                transform: ClassParser.GetRegistrationToGenerate);

        var jsonContexts =
            context.SyntaxProvider
            .ForAttributeWithMetadataName(
                GeneratedJsonSerializerContextAttribute.Constants.AttributeFullName,
                predicate: static (node, _) => node is ClassDeclarationSyntax,
                transform: ClassParser.GetRegistrationToGenerate);

        var registrationsCollected = registrations.Collect();
        var jsonContextsCollected = jsonContexts.Collect();

        var combined = registrationsCollected.Combine(jsonContextsCollected);

        // Generate single source code for all classes to add to json serialization
        context.RegisterSourceOutput(combined,
            static (spc, sources) => GenerateCode(sources, spc));
    }

    private static void GenerateCode((ImmutableArray<GenerateInfoBase?> Left, ImmutableArray<GenerateInfoBase?> Right) sources, SourceProductionContext context)
    {
        var infos = sources.Left.Concat(sources.Right);

        var groups = infos.Where(x => x is not null).GroupBy(x => x!.Key);

        foreach (var group in groups)
        {
            var jsonSourceGenerationInfo = group.FirstOrDefault(x => x is JsonSourceGenerationInfo) as JsonSourceGenerationInfo;

            if (jsonSourceGenerationInfo is null)
            {
                continue;
            }

            var registrations = group.OfType<RegistrationToGenerateInfo>().ToImmutableArray();

            var codeString = SourceGenerationHelper.GenerateSource(jsonSourceGenerationInfo, registrations);
            var sourceText = SourceText.From(codeString, Encoding.UTF8);
            context.AddSource($"{jsonSourceGenerationInfo.ClassName}.Registrations.g.cs", sourceText);
        }
    }
}
