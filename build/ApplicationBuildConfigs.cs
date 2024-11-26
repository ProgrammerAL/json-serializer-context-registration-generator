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
        var projectName = "JsonSerializerContextRegistrationGenerator";
        var pathToSln = srcDirectory + $"/{projectName}.sln";
        var attributesCsProjFile = $"{srcDirectory}/JsonSerializerContextRegistrationGenerator.Attributes.csproj";
        var runnerCsProjFile = $"{srcDirectory}/JsonSerializerContextRegistrationGenerator.Runner.csproj";
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
