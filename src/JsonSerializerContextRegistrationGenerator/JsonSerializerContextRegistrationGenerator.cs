using System.Text;

using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator.Attributes;
using ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator.GeneratorParsers;
using System.Collections.Immutable;

namespace ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator;

[Generator]
public class JsonSerializerContextRegistrationGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var toGenerate =
            context.SyntaxProvider
            .ForAttributeWithMetadataName(
                RegisterJsonSerializationAttribute.Constants.AttributeFullName,
                predicate: static (node, _) => node is ClassDeclarationSyntax or RecordDeclarationSyntax,
                transform: ClassParser.GetRegistrationToGenerate);

        var collectedClasses = toGenerate.Collect();

        // Generate single source code for all classes to add to json serialization
        context.RegisterSourceOutput(collectedClasses,
            static (spc, sources) => GenerateCode(sources, spc));
    }

    private static void GenerateCode(ImmutableArray<GenerateInfoBase?> infos, SourceProductionContext context)
    {
        if (infos.Length == 0)
        {
            return;
        }

        infos.GroupBy()

        //var codeString = SourceGenerationHelper.GenerateSource(in infos);
        //var sourceText = SourceText.From(codeString, Encoding.UTF8);
        //context.AddSource($"{info.SerializerContext.ClassName}.{info.Registration.ClassName}.Registration.g.cs", sourceText);
    }
}
