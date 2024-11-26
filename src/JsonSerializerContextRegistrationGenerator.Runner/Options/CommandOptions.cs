using CommandLine;

namespace ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator.Runner.Options;
public class CommandOptions
{
    [Option(shortName: 's', longName: "sources", Required = true, HelpText = "Path to the root directory of a set of code files")]
    public required string SourcesPath { get; set; }

    [Option(shortName: 'o', longName: "output", Required = true, HelpText = "Path to the directory to output files to")]
    public required string OutputPath { get; set; }
}
