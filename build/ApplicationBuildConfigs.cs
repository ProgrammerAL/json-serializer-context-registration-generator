using Cake.Common;
using Cake.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record ProjectPaths(
    string ProjectName,
    string PathToSln,
    string AttributesCsprojFile,
    string RunnerCsprojFile,
    string UnitTestProj,
    string OutDir,
    string NuGetFilePath)
{
    public static ProjectPaths LoadFromContext(ICakeContext context, string buildConfiguration, string srcDirectory, string nugetVersion)
    {
        var projectName = "JsonSerializerRegistrationGenerator";
        var pathToSln = srcDirectory + $"/{projectName}.sln";
        var attributesCsProjFile = $"{srcDirectory}/JsonSerializerRegistrationGenerator.Attributes/JsonSerializerRegistrationGenerator.Attributes.csproj";
        var runnerCsProjFile = $"{srcDirectory}/JsonSerializerRegistrationGenerator.Runner/JsonSerializerRegistrationGenerator.Runner.csproj";
        var unitTestsProj =  $"{srcDirectory}/UnitTests/UnitTests.csproj";
        var outDir = $"{srcDirectory}/cake-build-output";
        var nugetFilePath = outDir + $"/*{nugetVersion}.nupkg";

        return new ProjectPaths(
            projectName,
            pathToSln,
            attributesCsProjFile,
            runnerCsProjFile,
            unitTestsProj,
            outDir,
            nugetFilePath);
    }
};
