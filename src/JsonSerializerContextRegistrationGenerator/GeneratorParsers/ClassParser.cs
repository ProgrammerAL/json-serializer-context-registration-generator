using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;

using ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator.Attributes;

using static ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator.RegistrationToGenerateInfo;

namespace ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator.GeneratorParsers;

public static class ClassParser
{
    public static RegistrationToGenerateInfo? GetRegistrationToGenerate(GeneratorAttributeSyntaxContext context, CancellationToken ct)
    {
        var symbol = context.TargetSymbol as INamedTypeSymbol;
        if (symbol is null)
        {
            // nothing to do if this type isn't available
            return null;
        }

        ct.ThrowIfCancellationRequested();

        INamedTypeSymbol? jsonSerializerContextType = null;

        foreach (AttributeData attributeData in symbol.GetAttributes())
        {
            var attributeClassName = attributeData.AttributeClass?.Name;
            if (string.IsNullOrWhiteSpace(attributeClassName)
                || attributeClassName != RegisterJsonSerializationAttribute.Constants.AttributeName
                || attributeData.AttributeClass!.ToDisplayString() != RegisterJsonSerializationAttribute.Constants.AttributeFullName)
            {
                continue;
            }

            var constructorArgs = attributeData.ConstructorArguments;
            if (constructorArgs.Length != 1)
            {
                // Can't generate anything if the attribute isn't used correctly
                // Note: Is there a way to surface this to users?
                return null;
            }

            jsonSerializerContextType = constructorArgs[0].Value as INamedTypeSymbol;
        }

        if (jsonSerializerContextType is null)
        {
            // Can't generate anything if the attribute isn't used correctly
            // Note: Is there a way to surface this to users?
            return null;
        }

        return TryExtractSymbols(symbol, jsonSerializerContextType);
    }

    private static RegistrationToGenerateInfo? TryExtractSymbols(
        INamedTypeSymbol symbol,
        INamedTypeSymbol jsonSerializerContextType)
    {
        var contextNamespace = DetermineNamespace(jsonSerializerContextType);
        var contextName = jsonSerializerContextType.Name;
        var contextAccessibility = jsonSerializerContextType.DeclaredAccessibility switch
        {
            Accessibility.Public => "public",
            Accessibility.ProtectedOrInternal => "protected internal",
            Accessibility.Protected => "protected",
            Accessibility.Internal => "internal",
            Accessibility.Private => "private",
            _ => string.Empty
        };

        var serializerContextInfo = new SerializerContextInfo(contextNamespace, contextAccessibility, contextName);

        var symbolNamespace = DetermineNamespace(symbol);
        var symbolName = symbol.Name;

        var regInfo = new RegistrationInfo(symbolNamespace, symbolName);

        return new RegistrationToGenerateInfo(serializerContextInfo, regInfo);
    }

    private static string DetermineNamespace(INamedTypeSymbol symbol)
    {
        return symbol.ContainingNamespace.IsGlobalNamespace
            ? string.Empty
            : symbol.ContainingNamespace.ToString() ?? string.Empty;
    }
}
