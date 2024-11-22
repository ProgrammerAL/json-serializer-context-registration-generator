using System.Text;

using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator.Attributes;
using ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator.GeneratorParsers;

namespace ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator;

[Generator]
public class JsonSerializerContextRegistrationGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var interfacesToGenerate =
            context.SyntaxProvider
            .ForAttributeWithMetadataName(
                RegisterJsonSerializationAttribute.Constants.AttributeFullName,
                predicate: static (node, _) => node is ClassDeclarationSyntax or RecordDeclarationSyntax,
                transform: ClassParser.GetRegistrationToGenerate);

        // Generate source code for each interface
        context.RegisterSourceOutput(interfacesToGenerate,
            static (spc, source) => GenerateInterface(source, spc));
    }

    private static void GenerateInterface(in RegistrationToGenerateInfo? info, SourceProductionContext context)
    {
        if (info is null)
        {
            return;
        }

        var codeString = SourceGenerationHelper.GenerateInterface(in info);
        var sourceText = SourceText.From(codeString, Encoding.UTF8);
        context.AddSource($"{info.SerializerContext.ClassName}.{info.Registration.ClassName}.Registration.g.cs", sourceText);
    }
}
