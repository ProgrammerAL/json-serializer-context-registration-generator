using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

using Microsoft.CodeAnalysis;

using ProgrammerAL.JsonSerializerRegistrationGenerator.Attributes;

namespace ProgrammerAL.JsonSerializerRegistrationGenerator.GeneratorParsers;

public static class ClassParser
{
    public static GenerateInfoBase? GetRegistrationToGenerate(GeneratorAttributeSyntaxContext context, CancellationToken ct)
    {
        var symbol = context.TargetSymbol as INamedTypeSymbol;
        if (symbol is null)
        {
            // nothing to do if this type isn't available
            return null;
        }

        ct.ThrowIfCancellationRequested();

        var symbolAttributes = symbol.GetAttributes();
        foreach (AttributeData attributeData in symbolAttributes)
        {
            var attributeDisplayString = attributeData.AttributeClass?.ToDisplayString();
            if (string.IsNullOrWhiteSpace(attributeDisplayString))
            {
                continue;
            }

            if (attributeDisplayString == RegisterJsonSerializationAttribute.Constants.AttributeFullName)
            {
                return ExtractRegistrationToGenerateInfo(symbol, attributeData);
            }

            if (attributeDisplayString == GeneratedJsonSerializerContextAttribute.Constants.AttributeFullName)
            {
                return ExtractJsonSourceGenerationInfo(symbol);
            }
        }

        return null;
    }

    private static JsonSourceGenerationInfo? ExtractJsonSourceGenerationInfo(
        INamedTypeSymbol symbol)
    {
        var jsonSerializerContextType = symbol;

        return TryExtractJsonSourceGenerationInfoSymbols(symbol, jsonSerializerContextType);
    }

    private static RegistrationToGenerateInfo? ExtractRegistrationToGenerateInfo(INamedTypeSymbol symbol, AttributeData attributeData)
    {
        INamedTypeSymbol? jsonSerializerContextType = null;
        var genericTypes = ImmutableArray<string>.Empty;

        var constructorArgs = attributeData.ConstructorArguments;
        if (constructorArgs.Length == 1)
        {
            jsonSerializerContextType = constructorArgs[0].Value as INamedTypeSymbol;
        }
        else if (constructorArgs.Length == 2)
        {
            jsonSerializerContextType = constructorArgs[0].Value as INamedTypeSymbol;
            genericTypes = constructorArgs[1].Values.Select(x => x.Value?.ToString())
                .Where(x => x is not null)
                .Select(x => x!)
                .ToImmutableArray();
        }

        if (jsonSerializerContextType is null)
        {
            // Can't generate anything if the attribute isn't used correctly
            // Note: Is there a way to surface this to users?
            return null;
        }

        return TryExtractRegistrationToGenerateInfoSymbols(symbol, jsonSerializerContextType, genericTypes);
    }

    private static RegistrationToGenerateInfo? TryExtractRegistrationToGenerateInfoSymbols(
        INamedTypeSymbol symbol,
        INamedTypeSymbol jsonSerializerContextType,
        ImmutableArray<string> genericTypes)
    {
        var key = DetermineContextKey(jsonSerializerContextType);

        var symbolNamespace = DetermineNamespace(symbol);
        var symbolName = symbol.Name;

        return new RegistrationToGenerateInfo(symbolNamespace, symbolName, genericTypes, key);
    }

    private static JsonSourceGenerationInfo? TryExtractJsonSourceGenerationInfoSymbols(
        INamedTypeSymbol symbol,
        INamedTypeSymbol jsonSerializerContextType)
    {
        var key = DetermineContextKey(jsonSerializerContextType);

        var contextNamespace = DetermineNamespace(jsonSerializerContextType);
        var className = jsonSerializerContextType.Name;
        var accessibility = jsonSerializerContextType.DeclaredAccessibility switch
        {
            Accessibility.Public => "public",
            Accessibility.ProtectedOrInternal => "protected internal",
            Accessibility.Protected => "protected",
            Accessibility.Internal => "internal",
            Accessibility.Private => "private",
            _ => string.Empty
        };

        return new JsonSourceGenerationInfo(contextNamespace, accessibility, className, key);
    }

    private static string DetermineContextKey(INamedTypeSymbol jsonSerializerContextType)
    {
        var contextNamespace = DetermineNamespace(jsonSerializerContextType);
        return $"{contextNamespace}.{jsonSerializerContextType.Name}";
    }

    private static string DetermineNamespace(INamedTypeSymbol symbol)
    {
        return symbol.ContainingNamespace.IsGlobalNamespace
            ? string.Empty
            : symbol.ContainingNamespace.ToString() ?? string.Empty;
    }
}
